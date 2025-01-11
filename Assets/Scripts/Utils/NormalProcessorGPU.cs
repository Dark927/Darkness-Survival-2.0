using System;
using UnityEditor;
using UnityEngine;


public class NormalProcessorGPU : System.IDisposable
{
    private ComputeShader computeShader;
    //pipeline textures
    public Texture2D inputTexture  { get; private set; }
    public RenderTexture tempTexture { get; private set; }
    public RenderTexture outputTexture { get; private set; }

    //curve LUT
    private static readonly int resolution = 256;
    private Texture2D curveLUT = new(resolution, 1, TextureFormat.RFloat, false, true);

    public NormalProcessorGPU(Texture2D tex)
    {
        computeShader = Resources.Load<ComputeShader>("NormalMapComputeShader");

        // Create input and output textures
        /*inputTexture = new(inputTexture.width, inputTexture.height, 0, RenderTextureFormat.ARGBFloat)
        {
            enableRandomWrite = true
        };
        inputTexture.Create();
        Graphics.Blit(inputTexture, inputTexture);*/
        inputTexture = tex;
        GenTextures();
    }

    public void RebindTexture(Texture2D tex){
        //check dimensions
        if(inputTexture.width != tex.width || inputTexture.height != tex.height)
        {
            inputTexture = tex;
            Dispose();
            GenTextures();
        }
        else
        {
            inputTexture = tex;
        }
    }

    private void GenTextures()
    {
        tempTexture = new(inputTexture.width, inputTexture.height, 0, RenderTextureFormat.RFloat)
        {
            enableRandomWrite = true
        };
        tempTexture.Create();


        outputTexture = new(inputTexture.width, inputTexture.height, 0, RenderTextureFormat.ARGB32)
        {
            enableRandomWrite = true
        };
        outputTexture.Create();
    }

    internal void ComputeLUT(AnimationCurve curve)
    {
        for (int i = 0; i < resolution; i++)
        {
            float t = i / (float)(resolution - 1); // Normalize to [0, 1]
            float value = curve.Evaluate(t); // Sample the curve
            curveLUT.SetPixel(i, 0, new Color(value, 0, 0, 0));
        }
        curveLUT.Apply();
        //Debug.Log("Generated LUT");
    }

    public void ComputeGauss(float smoothness)
    {
        int gaussian = computeShader.FindKernel("GaussianBlur");

        // Set shader parameters
        computeShader.SetInt("_Width", inputTexture.width);
        computeShader.SetInt("_Height", inputTexture.height);
        computeShader.SetFloat("_Smoothness", smoothness);
        
        // Execute the compute shader
        int threadGroupsX = Mathf.CeilToInt(inputTexture.width / 8.0f);
        int threadGroupsY = Mathf.CeilToInt(inputTexture.height / 8.0f);
 
        computeShader.SetTexture(gaussian, "InputTexture", inputTexture);
        computeShader.SetTexture(gaussian, "TempTexture", tempTexture);
        computeShader.SetTexture(gaussian, "OutputTexture", outputTexture);
        computeShader.SetTexture(gaussian, "CurveLUTTexture", curveLUT);
        computeShader.Dispatch(gaussian, threadGroupsX, threadGroupsY, 1);
        //Debug.Log("Gaussian blur applied");
    }

    public void ComputeNormal(float intensity)
    {
        int sobel = computeShader.FindKernel("NormalKernel");
        computeShader.SetInt("_Width", inputTexture.width);
        computeShader.SetInt("_Height", inputTexture.height);
        computeShader.SetFloat("_Intensity", intensity);

        // Execute the compute shader
        int threadGroupsX = Mathf.CeilToInt(inputTexture.width / 8.0f);
        int threadGroupsY = Mathf.CeilToInt(inputTexture.height / 8.0f);
        
        computeShader.SetTexture(sobel, "InputTexture", inputTexture);
        computeShader.SetTexture(sobel, "TempTexture", tempTexture);
        computeShader.SetTexture(sobel, "OutputTexture", outputTexture);
        computeShader.SetTexture(sobel, "CurveLUTTexture", curveLUT);
        computeShader.Dispatch(sobel, threadGroupsX, threadGroupsY, 1);
        //Debug.Log("Normal computation applied");
    }

    public Texture2D GetTexture()
    {
        // Convert the output texture to Texture2D
        Texture2D result = new(inputTexture.width, inputTexture.height, TextureFormat.RGB24, false);
        RenderTexture.active = outputTexture;
        result.ReadPixels(new Rect(0, 0, outputTexture.width, outputTexture.height), 0, 0);
        result.Apply();
        
        RenderTexture.active = null;
        return result;
    }

    public void Dispose()
    {
        if (tempTexture != null)
        {
            tempTexture.Release();
        }
        if (outputTexture != null)
        {
            outputTexture.Release();
        }
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

        using var processor = new NormalProcessorGPU(texture);
        Texture2D normalMap = processor.GetTexture();

        // Save the normal map texture
        string assetPath = AssetDatabase.GetAssetPath(texture);
        string normalMapPath = assetPath.Replace(".png", "_Normal.png");

        // Save the normal map to disk
        System.IO.File.WriteAllBytes(normalMapPath, normalMap.EncodeToPNG());
        AssetDatabase.Refresh();
    }
}
