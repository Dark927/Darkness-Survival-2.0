#ifndef DARK_FX_INCLUDED
#define DARK_FX_INCLUDED

#define RENDMODE_ORIGINAL 1
#define RENDMODE_NOFX 2
#define RENDMODE_UNLIT 3

#define DARKFX_NONE 0
#define DARKFX_FLASH 1
#define DARKFX_EMISSION 2
#define DARKFX_DISSOLVE 4
#define DARKFX_HSVREPLACE 8
#define DARKFX_HSVINTERP 16
#define DARKFX_HSVMASK 32
#define DARKFX_JITTERFREE 64
#define DARKFX_TINT 128

//Flash
float4 _FlashColor;
float _FlashAmount;

//Emission
TEXTURE2D(_EmissionMaskTex);
SAMPLER(sampler_EmissionMaskTex);
float4 _EmissionColor;
float _EmissionAmount;

//Dissolve
float _Seed;
float _DissolveNoise;
float4 _DissolveColor;
float _DissolveAmount;
float _DissolveOutline;

//HSV Replace
TEXTURE2D(_HSVMaskTex);
SAMPLER(sampler_HSVMaskTex);
float4 _TargetColor;
uniform float4x4 _HsvaK;
uniform float4 _HsvaB;
float _HueTolerance;
float _SatTolerance;
float _ValTolerance;
float _HSVGamma;

//Tint
float _Gamma;
//float4 _Color (built-in)

//Misc
int _RendMode;
int _FXFeatures;

float4 ProcessHSV(float4 col)
{
    float3 hsv = rgb_to_hsv(col.rgb);
    float3 targetHSV = rgb_to_hsv(_TargetColor.rgb); //TODO: move to c#

    // Calculate hue difference with wrap-around
    float hueDiff = abs(hsv.x - targetHSV.x);
    hueDiff = min(hueDiff, 1.0 - hueDiff);

    // Create replacement color version
    float4 replacedHsv = float4(hsv, col.a);                
    replacedHsv = mul(_HsvaK, replacedHsv) + _HsvaB;
    float3 replacedRGB = hsv_to_rgb(replacedHsv.xyz);
    
    //INTERP
    if(_RendMode & DARKFX_HSVINTERP)
    {
        // Calculate smooth transition factors for each component
        float hueBlend = 1 - smoothstep(0, _HueTolerance, hueDiff);
        float satBlend = 1 - smoothstep(0, _SatTolerance, abs(hsv.y - targetHSV.y));
        float valBlend = 1 - smoothstep(0, _ValTolerance, abs(hsv.z - targetHSV.z));
        
        // Combine all blend factors
        float totalBlend = hueBlend * satBlend * valBlend;
        totalBlend = pow(abs(totalBlend), 1.0 / _HSVGamma);

        // Smoothly interpolate between original and replaced color
        float3 interpolatedRGB = lerp(col.rgb, replacedRGB.rgb, saturate(totalBlend));

        return float4(interpolatedRGB, replacedHsv.a);
    }
    //STEP
    else
    {    
        if (hueDiff <= _HueTolerance &&
            abs(hsv.y - targetHSV.y) <= _SatTolerance &&
            abs(hsv.z - targetHSV.z) <= _ValTolerance)
        { 
            return float4(replacedRGB.rgb, replacedHsv.a);
        }
        return col;
    }
}


void ApplyDarkFX(inout float4 main, uniform float4 vertColor, uniform float2 uv, uniform float2 origUV)
{
    [branch]
    if(_RendMode == RENDMODE_NOFX)
    {
        return;
    }

    [branch]
    if(_FXFeatures & DARKFX_HSVREPLACE)
    {
        main = ProcessHSV(main);
    }

    [branch]
    if(_FXFeatures & DARKFX_TINT)
    {
        main.rgb = pow(abs(main.rgb * vertColor.rgb), _Gamma);
    }

    [branch]
    if(_FXFeatures & DARKFX_FLASH)
    {
        main.rgb = lerp(main.rgb, _FlashColor.rgb, _FlashAmount);
    }

    [branch]
    if(_FXFeatures & DARKFX_EMISSION)
    {
        float emissionCoef = SAMPLE_TEXTURE2D(_EmissionMaskTex, sampler_EmissionMaskTex, uv).r;
        main.rgb += _EmissionColor.rgb * 1 * _EmissionAmount;
    }

    [branch]
    if(_FXFeatures & DARKFX_DISSOLVE)
    {
        //uncomment to make noise fixed during animations.
        //origUV.x = frac(origUV.x * 4);
        //origUV.y = frac(origUV.y * 3);
        float noiseTex = smoothstep(0, 1, noise(origUV.xy * _DissolveNoise, _Seed));
        float dissolve = step(noiseTex, _DissolveAmount);
        float dissolveOutline = step(noiseTex, _DissolveAmount * _DissolveOutline);
        float dissolveDiff = dissolveOutline - dissolve;

        main.rgb = lerp(main.rgb, _DissolveColor, dissolveDiff);
        main.a = saturate(main.a * (1 - dissolve));
    }

}



#endif