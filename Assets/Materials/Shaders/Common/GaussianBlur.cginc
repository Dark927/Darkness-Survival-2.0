void GaussianBlur_float(UnityTexture2D Texture, float2 UV, float Blur, UnitySamplerState Sampler, out float3 Out_RGB, out float Out_Alpha)
{
    float4 col = float4(0.0, 0.0, 0.0, 0.0);
    float kernelSum = 0.0;

    int upper = ((Blur - 1) / 2);
    int lower = -upper;

    // Scaling factor (avoid tiling by clamping UVs within [0,1])
    float2 texelSize = _MainTex_TexelSize;

    for (int x = lower; x <= upper; ++x)
    {
        for (int y = lower; y <= upper; ++y)
        {
            kernelSum++;

            // Apply the kernel offsets based on the texture resolution
            float2 offset = float2(texelSize.x * x, texelSize.y * y);
            // Prevent UVs from tiling by clamping them between 0 and 1
            float2 sampleUV = clamp(UV + offset, 0.0, 1.0);

            col += Texture.Sample(Sampler, sampleUV);
        }
    }

    col /= kernelSum;
    Out_RGB = float3(col.r, col.g, col.b);
    Out_Alpha = col.a;
}
