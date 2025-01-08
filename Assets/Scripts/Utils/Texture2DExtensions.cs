using System;
using UnityEngine;

namespace Dark.Utils
{

    public static class Texture2DExtensions
    {
        public static void DrawBorder(this Texture2D tex, Color borderCol, int thickness)
        {
            if (tex.format != TextureFormat.RGBA32)
                throw new Exception("WRONG TEXTURE FORMAT TO ADD BORDER!");

            int width = tex.width;
            int height = tex.height;

            // Draw the top and bottom borders
            for (int y = 0; y < thickness; y++) // Top border
            {
                for (int x = 0; x < width; x++)
                {
                    tex.SetPixel(x, y, borderCol);
                }
            }
            for (int y = height - thickness; y < height; y++) // Bottom border
            {
                for (int x = 0; x < width; x++)
                {
                    tex.SetPixel(x, y, borderCol);
                }
            }

            // Draw the left and right borders
            for (int x = 0; x < thickness; x++) // Left border
            {
                for (int y = 0; y < height; y++)
                {
                    tex.SetPixel(x, y, borderCol);
                }
            }
            for (int x = width - thickness; x < width; x++) // Right border
            {
                for (int y = 0; y < height; y++)
                {
                    tex.SetPixel(x, y, borderCol);
                }
            }

            tex.Apply(); // Apply changes to the texture
        }
    }
}