using UnityEditor;
using UnityEngine;

public static class NormalProcessor
{
    public static Texture2D ProcessTexture(Texture2D inputTexture, float smoothness, float intensity)
    {
        // Convert texture to grayscale
        float[,] grayscale = ConvertToGrayscale(inputTexture);

        // Apply Gaussian smoothing
        //float[,] smoothed = ApplyGaussianSmoothing(grayscale, smoothness);

        // Compute gradients
        (float[,] sobelX, float[,] sobelY) = ApplySobel(grayscale);

        // Compute normal map
        Texture2D normalMap = ComputeNormalMap(sobelX, sobelY, intensity);

        return normalMap;
    }

    private static float[,] ConvertToGrayscale(Texture2D texture)
    {
        int width = texture.width;
        int height = texture.height;
        float[,] grayscale = new float[height, width];

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                Color pixel = texture.GetPixel(x, y);
                grayscale[y, x] = pixel.r * 0.3f + pixel.g * 0.6f + pixel.b * 0.1f;
            }
        }

        return grayscale;
    }

    private static float[,] ApplyGaussianSmoothing(float[,] image, float sigma)
    {
        int radius = Mathf.CeilToInt(3 * sigma);
        int size = 2 * radius + 1;

        float[] kernel = new float[size];
        float kernelSum = 0;

        for (int i = -radius; i <= radius; i++)
        {
            kernel[i + radius] = Mathf.Exp(-i * i / (2 * sigma * sigma));
            kernelSum += kernel[i + radius];
        }

        for (int i = 0; i < size; i++)
        {
            kernel[i] /= kernelSum;
        }

        int width = image.GetLength(1);
        int height = image.GetLength(0);
        float[,] smoothed = new float[height, width];

        // Horizontal pass with wrapping
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                float sum = 0;
                for (int k = -radius; k <= radius; k++)
                {
                    int wrappedX = (x + k + width) % width;
                    sum += image[y, wrappedX] * kernel[k + radius];
                }
                smoothed[y, x] = sum;
            }
        }

        // Vertical pass with wrapping
        float[,] result = new float[height, width];
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                float sum = 0;
                for (int k = -radius; k <= radius; k++)
                {
                    int wrappedY = (y + k + height) % height;
                    sum += smoothed[wrappedY, x] * kernel[k + radius];
                }
                result[y, x] = sum;
            }
        }

        return result;
    }

    private static (float[,], float[,]) ApplySobel(float[,] image)
    {
        int width = image.GetLength(1);
        int height = image.GetLength(0);

        float[,] sobelX = new float[height, width];
        float[,] sobelY = new float[height, width];

        int[,] kernelX = { { -1, 0, 1 }, { -2, 0, 2 }, { -1, 0, 1 } };
        int[,] kernelY = { { -1, -2, -1 }, { 0, 0, 0 }, { 1, 2, 1 } };

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                float gx = 0, gy = 0;

                for (int ky = -1; ky <= 1; ky++)
                {
                    for (int kx = -1; kx <= 1; kx++)
                    {
                        int wrappedX = (x + kx + width) % width;
                        int wrappedY = (y + ky + height) % height;

                        gx += image[wrappedY, wrappedX] * kernelX[ky + 1, kx + 1];
                        gy += image[wrappedY, wrappedX] * kernelY[ky + 1, kx + 1];
                    }
                }

                sobelX[y, x] = gx;
                sobelY[y, x] = gy;
            }
        }

        return (sobelX, sobelY);
    }

    private static Texture2D ComputeNormalMap(float[,] gradientX, float[,] gradientY, float intensity)
    {
        int width = gradientX.GetLength(1);
        int height = gradientX.GetLength(0);

        Texture2D normalMap = new Texture2D(width, height);

        float maxGradient = 1e-5f;
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                maxGradient = Mathf.Max(maxGradient, Mathf.Abs(gradientX[y, x]), Mathf.Abs(gradientY[y, x]));
            }
        }


        intensity = 1.0f / intensity;

        var strength = maxGradient / (maxGradient * intensity);

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                float dx = gradientX[y, x] / maxGradient;
                float dy = gradientY[y, x] / maxGradient;
                float dz = 1.0f / strength;

                Vector3 normal = new Vector3(dx, dy, dz).normalized;
                Color normalColor = new Color(normal.x * 0.5f + 0.5f, normal.y * 0.5f + 0.5f, normal.z * 0.5f + 0.5f);
                normalMap.SetPixel(x, y, normalColor);
            }
        }

        normalMap.Apply();
        return normalMap;
    }
}


public static class MenuUtilsNormie
{
    [MenuItem("Assets/Dark/Generate Normals", true)]
    public static bool ValidateOpenPreview()
    {
        return Selection.activeObject is Texture2D;
    }

    [MenuItem("Assets/Dark/Generate Normals")]
    public static void OpenPreview()
    {
        Texture2D texture = Selection.activeObject as Texture2D;
        if (texture == null)
        {
            EditorUtility.DisplayDialog("Error", $"Please select a valid Texture, you've selected {Selection.activeObject.GetType().FullName}", "OK");
            return;
        }

        Texture2D normalMap = NormalProcessor.ProcessTexture(texture, smoothness: 2f, intensity: 4f);

        // Save the normal map texture
        string assetPath = AssetDatabase.GetAssetPath(texture);
        string normalMapPath = assetPath.Replace(".png", "_Normal.png");

        // Save the normal map to disk
        System.IO.File.WriteAllBytes(normalMapPath, normalMap.EncodeToPNG());
        AssetDatabase.Refresh();


    }
}
