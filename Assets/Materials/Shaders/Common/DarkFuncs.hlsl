// Jitter-Free Sampling

#ifdef DARK_COMMON_LIT_INCLUDED
float2 TexturePointSmoothUV(float2 uvs)
{
    uvs -= _MainTex_TexelSize.xy * 0.5;
    float2 uv_pixels = uvs * _MainTex_TexelSize.zw;
    float2 delta_pixel = frac(uv_pixels) - 0.5;
    float2 ddxy = fwidth(uv_pixels);
    return uvs + (clamp(delta_pixel / ddxy, 0.0, 1.0) - delta_pixel) * _MainTex_TexelSize.xy;
}
#endif

float3 rgb_to_hsv(float3 rgb)
{
    float4 K = float4(0.0, -1.0 / 3.0, 2.0 / 3.0, -1.0);
    float4 p = lerp(float4(rgb.bg, K.wz), float4(rgb.gb, K.xy), step(rgb.b, rgb.g));
    float4 q = lerp(float4(p.xyw, rgb.r), float4(rgb.r, p.yzx), step(p.x, rgb.r));
    float d = q.x - min(q.w, q.y);
    float e = 1.0e-10;
    return float3(abs(q.z + (q.w - q.y) / (6.0 * d + e)), d / (q.x + e), q.x);
}

float3 hsv_to_rgb(float3 hsv)
{
    float4 K = float4(1.0, 2.0 / 3.0, 1.0 / 3.0, 3.0);
    float3 p = abs(frac(hsv.xxx + K.xyz) * 6.0 - K.www);
    return hsv.z * lerp(K.xxx, saturate(p - K.xxx), hsv.y);
}