/////////////////////
// Texturing utils //
/////////////////////

// Jitter - Free Sampling
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


/////////////////
// Color utils //
/////////////////

float3 rgb_to_hsv(float3 rgb)
{
    float4 K = float4(0.0, -1.0 / 3.0, 2.0 / 3.0, -1.0);
    float4 p = lerp(float4(rgb.bg, K.wz), float4(rgb.gb, K.xy), step(rgb.b, rgb.g));
    float4 q = lerp(float4(p.xyw, rgb.r), float4(rgb.r, p.yzx), step(p.x, rgb.r));
    float d = q.x - min(q.w, q.y);
    float e = 1e-10;
    return float3(abs(q.z + (q.w - q.y) / (6.0 * d + e)), d / (q.x + e), q.x);
}

float3 hsv_to_rgb(float3 hsv)
{
    float4 K = float4(1.0, 2.0 / 3.0, 1.0 / 3.0, 3.0);
    float3 p = abs(frac(hsv.xxx + K.xyz) * 6.0 - K.www);
    return hsv.z * lerp(K.xxx, saturate(p - K.xxx), hsv.y);
}


/////////////////////
// Misc Math utils //
/////////////////////

float gaussian(float x, float sigma)
{
    return exp(-0.5 * (x * x) / (sigma * sigma));
}

float3 mod(float3 x, float3 y)
{
    return x - y * floor(x / y);
}

float rand(float2 n)
{
    return frac(sin(dot(n, float2(12.9898, 4.1414))) * 43758.5453);
}

float rand(float2 n, float seed)
{
    return frac(sin(dot(n, float2(12.9898 * frac(seed), 4.1414 + frac(seed /  42)))) * 43758.5453);
}

float ndot(float2 a, float2 b) {
    return a.x * b.x - a.y * b.y;
}

float InverseLerp(float a, float b, float v)
{
    return (v - a) / (b - a);
}

//dont use for precise things
float fastSqrt(float x) {
    int i = asint(x);
    i = (i >> 1) + (0x1FC00000);
    return asfloat(i);
}


//////////////////////
// Misc Noise funcs //
//////////////////////

// https://thebookofshaders.com/11/


float noise(float2 p, float seed = 0) {
    float2 ip = floor(p);
    float2 fp = frac(p);
    fp = fp * fp * (3.0 - 2.0 * fp);

    return lerp(
    lerp(rand(ip,seed),                rand(ip + float2(1, 0),seed), fp.x),
    lerp(rand(ip + float2(0, 1),seed), rand(ip + float2(1, 1),seed), fp.x),
    fp.y
    );
}


float2 hash2D(float2 p) {
    p = float2(dot(p, float2(127.1, 311.7)),
    dot(p, float2(269.5, 183.3)));
    return frac(sin(p) * 43758.5453);
}


float3 voronoi(float2 uv) {
    const float scale = 10.0; // Adjust scale for size of Voronoi cells
    uv *= scale;

    float2 cell = floor(uv); // Current cell position
    float2 fractUV = frac(uv); // Fractional part of UV
    float minDist = 1.0; // Track the minimum distance
    float3 color = float3(0.0, 0.0, 0.0); // Output color

    // Loop through neighboring cells
    for (int y = -1; y <= 1; ++y) {
        for (int x = -1; x <= 1; ++x) {
            float2 neighborCell = cell + float2(x, y);

            // Random point in the neighboring cell
            float2 randomPoint = hash2D(neighborCell);

            // Compute the vector to this random point
            float2 pointVec = neighborCell + randomPoint - uv;

            // Compute the squared distance
            float dist = dot(pointVec, pointVec);

            // Keep track of the nearest point
            if (dist < minDist) {
                minDist = dist;
                color = float3(randomPoint, frac(randomPoint.x * randomPoint.y));
            }
        }
    }

    // The brightness is based on the distance
    return float3(color * minDist);
}


/////////////////////////////////////
// SDF (Signed distance functions) //
/////////////////////////////////////

// https://iquilezles.org/articles/distfunctions2d/
// https://mercury.sexy/hg_sdf/

float sdBox(float2 p, float2 b)
{
    float2 d = abs(p) - b;
    return length(max(d, 0.0)) + min(max(d.x, d.y), 0.0);
}

float sdCircle(float2 p, float r)
{
    return length(p) - r;
}

float sdRhombus(float2 p, in float2 b)
{
    p = abs(p);
    float h = clamp(ndot(b - 2.0 * p, b) / dot(b, b), - 1.0, 1.0);
    float d = length(p - 0.5 * b * float2(1.0 - h, 1.0 + h));
    return d * sign(p.x * b.y + p.y * b.x - b.x * b.y);
}


/////////////
// Effects //
/////////////

struct MagmaParams {
    float SPEED;
    float SCALE;
    float TURB;
    float CONTRAST;
    float2 DIR;
    float t;
};


float magma(float2 uv, MagmaParams params) {
    params.t *= params.SPEED;
    uv = uv * params.SCALE + params.DIR * params.t;

    float v = 0.0;
    v += noise(uv * 1.0 + noise(uv * 2.0)) * params.TURB;
    v += noise(uv * 4.0 - v * 0.5) * 0.4;
    v = saturate(abs(v * params.CONTRAST - 1.0));

    return smoothstep(0.2, 0.7, v) * 0.2 + 0.6;
}