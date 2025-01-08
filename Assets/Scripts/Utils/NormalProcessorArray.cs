using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;


public class NormalProcessorArray : System.IDisposable
{
    private ComputeShader computeShader;
    private RenderTexture outputTexture;

    public NormalProcessorArray()
    {
        computeShader = Resources.Load<ComputeShader>("NormalMapComputeShaderArray");
    }

    public Texture2DArray ProcessTexture(Texture2DArray inputTexture, float smoothness, float intensity)
    {
        int gaussian = computeShader.FindKernel("GaussianBlur");
        int sobel = computeShader.FindKernel("NormalKernel");

        // Create input and output textures
        RenderTexture inputRT = new(inputTexture.width, inputTexture.height, inputTexture.depth, RenderTextureFormat.ARGBFloat)
        {
            enableRandomWrite = true,
            dimension = UnityEngine.Rendering.TextureDimension.Tex2DArray, // Set as 2D texture array
            volumeDepth = inputTexture.depth // Number of slices in the texture array
        };
        inputRT.Create();
        Graphics.Blit(inputTexture, inputRT);


        RenderTexture tempTexture = new(inputTexture.width, inputTexture.height, inputTexture.depth, RenderTextureFormat.RFloat)
        {
            enableRandomWrite = true,
            dimension = UnityEngine.Rendering.TextureDimension.Tex2DArray, // Set as 2D texture array
            volumeDepth = inputTexture.depth // Number of slices in the texture array
        };
        tempTexture.Create();


        outputTexture = new(inputTexture.width, inputTexture.height, inputTexture.depth, RenderTextureFormat.ARGBFloat)
        {
            enableRandomWrite = true,
            dimension = UnityEngine.Rendering.TextureDimension.Tex2DArray, // Set as 2D texture array
            volumeDepth = inputTexture.depth // Number of slices in the texture array
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
        //int threadGroupsY = Mathf.CeilToInt(inputTexture.depth / 8.0f);

        computeShader.SetTexture(gaussian, "InputTexture", inputRT);
        computeShader.SetTexture(gaussian, "TempTexture", tempTexture);
        computeShader.SetTexture(gaussian, "OutputTexture", outputTexture);
        computeShader.Dispatch(gaussian, threadGroupsX, threadGroupsY, inputTexture.depth);

        computeShader.SetTexture(sobel, "InputTexture", inputRT);
        computeShader.SetTexture(sobel, "TempTexture", tempTexture);
        computeShader.SetTexture(sobel, "OutputTexture", outputTexture);
        computeShader.Dispatch(sobel, threadGroupsX, threadGroupsY, inputTexture.depth);

        // Convert the output texture to Texture2DArray
        Texture2DArray result = new(inputTexture.width, inputTexture.height, inputTexture.depth, TextureFormat.RGBAFloat, false);
        RenderTexture.active = outputTexture;
        //Graphics.CopyTexture(outputTexture, result);
        var asyncAction = AsyncGPUReadback.Request(outputTexture, 0);
        asyncAction.WaitForCompletion();
        for (int i = 0; i < 64; i++)
            result.SetPixelData(asyncAction.GetData<byte>(i), 0, i);
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


public static class MenuUtilsNormieArray
{
    [MenuItem("Assets/Dark/Generate Normals (Array)", true)]
    public static bool ValidateOpenPreview()
    {
        return Selection.activeObject is Texture2DArray;
    }

    [MenuItem("Assets/Dark/Generate Normals (Array)")]
    public static void OpenPreview()
    {
        Texture2DArray texture = Selection.activeObject as Texture2DArray;
        if (texture == null)
        {
            EditorUtility.DisplayDialog("Error", $"Please select a valid Texture, you've selected {Selection.activeObject.GetType().FullName}", "OK");
            return;
        }

        using var processor = new NormalProcessorArray();
        Texture2DArray normalMap = processor.ProcessTexture(texture, smoothness: 2f, intensity: 0.4f);

        // Save the normal map texture
        string assetPath = AssetDatabase.GetAssetPath(texture);
        string normalMapPath = assetPath.Replace(".png", "_Normal.png");

        // Save the normal map to disk
        SaveTextureAtlas(normalMap, 8, normalMapPath);

        AssetDatabase.Refresh();
    }

    public static void SaveTextureAtlas(Texture2DArray textureArray, int atlasColumns, string savePath)
    {
        int layerCount = textureArray.depth;
        int layerWidth = textureArray.width;
        int layerHeight = textureArray.height;

        // Calculate grid size
        int atlasRows = Mathf.CeilToInt((float)layerCount / atlasColumns);
        int atlasWidth = atlasColumns * layerWidth;
        int atlasHeight = atlasRows * layerHeight;

        // Create atlas texture
        Texture2D atlasTexture = new Texture2D(atlasWidth, atlasHeight, textureArray.format, false);

        // Copy each layer into the atlas
        for (int layer = 0; layer < layerCount; layer++)
        {
            int xOffset = (layer % atlasColumns) * layerWidth;
            int yOffset = ((layerCount - layer - 1) / atlasColumns) * layerHeight;

            // Extract layer
            Texture2D layerTexture = new Texture2D(layerWidth, layerHeight, textureArray.format, false);
            Graphics.CopyTexture(textureArray, layer, 0, layerTexture, 0, 0);

            // Copy pixels into atlas
            Color[] pixels = layerTexture.GetPixels();
            atlasTexture.SetPixels(xOffset, yOffset, layerWidth, layerHeight, pixels);
        }

        // Apply changes to atlas
        atlasTexture.Apply();

        // Encode to PNG
        byte[] pngData = atlasTexture.EncodeToPNG();

        // Save PNG
        System.IO.File.WriteAllBytes(savePath, pngData);

        Debug.Log($"Texture atlas saved to: {savePath}");
    }
}
