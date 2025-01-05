using System;
using UnityEditor;
using UnityEngine;


public class NormalProcessorGPU : System.IDisposable
{
    private ComputeShader computeShader;
    private RenderTexture outputTexture;

    public NormalProcessorGPU()
    {
        computeShader = Resources.Load<ComputeShader>("NormalMapComputeShader");
    }

    public Texture2D ProcessTexture(Texture2D inputTexture, float smoothness, float intensity)
    {
        int gaussian = computeShader.FindKernel("GaussianBlur");
        int sobel = computeShader.FindKernel("NormalKernel");

        // Create input and output textures
        RenderTexture inputRT = new(inputTexture.width, inputTexture.height, 0, RenderTextureFormat.ARGBFloat)
        {
            enableRandomWrite = true
        };
        inputRT.Create();
        Graphics.Blit(inputTexture, inputRT);


        RenderTexture tempTexture = new(inputTexture.width, inputTexture.height, 0, RenderTextureFormat.RFloat)
        {
            enableRandomWrite = true
        };
        tempTexture.Create();


        outputTexture = new(inputTexture.width, inputTexture.height, 0, RenderTextureFormat.ARGBFloat)
        {
            enableRandomWrite = true
        };
        outputTexture.Create();



        // Set shader parameters
        computeShader.SetInt("_Width", inputTexture.width);
        computeShader.SetInt("_Height", inputTexture.height);
        computeShader.SetFloat("_Smoothness", smoothness);
        computeShader.SetFloat("_Intensity", intensity);

        // Execute the compute shader
        int threadGroupsX = Mathf.CeilToInt(inputTexture.width / 8.0f);
        int threadGroupsY = Mathf.CeilToInt(inputTexture.height / 8.0f);

        computeShader.SetTexture(gaussian, "InputTexture", inputRT);
        computeShader.SetTexture(gaussian, "TempTexture", tempTexture);
        computeShader.SetTexture(gaussian, "OutputTexture", outputTexture);
        computeShader.Dispatch(gaussian, threadGroupsX, threadGroupsY, 1);

        computeShader.SetTexture(sobel, "InputTexture", inputRT);
        computeShader.SetTexture(sobel, "TempTexture", tempTexture);
        computeShader.SetTexture(sobel, "OutputTexture", outputTexture);
        computeShader.Dispatch(sobel, threadGroupsX, threadGroupsY, 1);

        // Convert the output texture to Texture2D
        Texture2D result = new(inputTexture.width, inputTexture.height, TextureFormat.RGBAFloat, false);
        RenderTexture.active = outputTexture;
        result.ReadPixels(new Rect(0, 0, outputTexture.width, outputTexture.height), 0, 0);
        result.Apply();
        
        RenderTexture.active = null;

        // Cleanup
        inputRT.Release();
        return result;
    }

    public void Dispose()
    {
        if (outputTexture != null)
        {
            outputTexture.Release();
        }
        computeShader = null;
    }
}


public static class MenuUtilsNormieGPU
{
    [MenuItem("Assets/Dark/Generate Normals (GPU)", true)]
    public static bool ValidateOpenPreview()
    {
        return Selection.activeObject is Texture2D;
    }

    [MenuItem("Assets/Dark/Generate Normals (GPU)")]
    public static void OpenPreview() 
    {
        Texture2D texture = Selection.activeObject as Texture2D;
        if (texture == null)
        {
            EditorUtility.DisplayDialog("Error", $"Please select a valid Texture, you've selected {Selection.activeObject.GetType().FullName}", "OK");
            return;
        }

        using var processor = new NormalProcessorGPU();
        Texture2D normalMap = processor.ProcessTexture(texture, smoothness: 2f, intensity: 0.4f);

        // Save the normal map texture
        string assetPath = AssetDatabase.GetAssetPath(texture);
        string normalMapPath = assetPath.Replace(".png", "_Normal.png");

        // Save the normal map to disk
        System.IO.File.WriteAllBytes(normalMapPath, normalMap.EncodeToPNG());
        AssetDatabase.Refresh();

        
    }
}
