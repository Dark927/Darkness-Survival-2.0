#ifndef DARK_COMMON_LIT_INCLUDED
#define DARK_COMMON_LIT_INCLUDED

#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
#include "Packages/com.unity.render-pipelines.universal/Shaders/2D/Include/LightingUtility.hlsl"
#include "Packages/com.unity.render-pipelines.universal/Shaders/2D/Include/NormalsRenderingShared.hlsl"

// Shared Textures
TEXTURE2D(_MainTex);
SAMPLER(sampler_MainTex);
float4 _MainTex_TexelSize;
float4 _MainTex_ST;
TEXTURE2D(_MaskTex);
SAMPLER(sampler_MaskTex);

TEXTURE2D(_NormalMap);
SAMPLER(sampler_NormalMap);
half4 _NormalMap_ST;

// Shared Properties
float4 _Color;
half4 _RendererColor;


// Light
#if USE_SHAPE_LIGHT_TYPE_0
SHAPE_LIGHT(0)
#endif

#if USE_SHAPE_LIGHT_TYPE_1
SHAPE_LIGHT(1)
#endif

#if USE_SHAPE_LIGHT_TYPE_2
SHAPE_LIGHT(2)
#endif

#if USE_SHAPE_LIGHT_TYPE_3
SHAPE_LIGHT(3)
#endif

// Yes, it must be here and not any higher
#include "Packages/com.unity.render-pipelines.universal/Shaders/2D/Include/CombinedShapeLightShared.hlsl"

// Universal2D Pass Structs
struct DarkAttributes2D
{
    float3 positionOS : POSITION;
    float4 color : COLOR;
    float2 uv : TEXCOORD0;
    UNITY_VERTEX_INPUT_INSTANCE_ID
};

struct DarkVaryings2D
{
    float4 positionCS : SV_POSITION;
    half4 color : COLOR;
    float2 uv : TEXCOORD0;
    half2 lightingUV : TEXCOORD1;
    #if defined(DEBUG_DISPLAY)
    float3 positionWS : TEXCOORD2;
    #endif
    UNITY_VERTEX_OUTPUT_STEREO
};

// Normals Pass Structs
struct DarkAttributesNormals
{
    float3 positionOS : POSITION;
    float4 color : COLOR;
    float2 uv : TEXCOORD0;
    float4 tangent : TANGENT;
    UNITY_VERTEX_INPUT_INSTANCE_ID
};

struct DarkVaryingsNormals
{
    float4 positionCS : SV_POSITION;
    half4 color : COLOR;
    float2 uv : TEXCOORD0;
    half3 normalWS : TEXCOORD1;
    half3 tangentWS : TEXCOORD2;
    half3 bitangentWS : TEXCOORD3;
    UNITY_VERTEX_OUTPUT_STEREO
};

// Load Dark utils
#include "DarkFuncs.hlsl"



// Dummy functions
DarkVaryings2D DarkVertex(DarkAttributes2D attributes)
{
    DarkVaryings2D o = (DarkVaryings2D)0;
    UNITY_SETUP_INSTANCE_ID(attributes);
    UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

    o.positionCS = TransformObjectToHClip(attributes.positionOS);
    #if defined(DEBUG_DISPLAY)
    o.positionWS = TransformObjectToWorld(attributes.positionOS);
    #endif
    o.uv = TRANSFORM_TEX(attributes.uv, _MainTex);
    o.lightingUV = half2(ComputeScreenPos(o.positionCS / o.positionCS.w).xy);
    o.color = attributes.color * _Color * _RendererColor;
    return o;
}

//used only in UniversalForward. Unlit
float4 DarkUnlitFragment(DarkVaryings2D i) : SV_Target
{
    float4 mainTex = i.color * SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv);

    #if defined(DEBUG_DISPLAY)
    SurfaceData2D surfaceData;
    InputData2D inputData;
    half4 debugColor = 0;

    InitializeSurfaceData(mainTex.rgb, mainTex.a, surfaceData);
    InitializeInputData(i.uv, inputData);
    SETUP_DEBUG_DATA_2D(inputData, i.positionWS);

    if(CanDebugOverrideOutputColor(surfaceData, inputData, debugColor))
    {
        return debugColor;
    }
    #endif

    return mainTex;
}

// NormalsRendering 
DarkVaryingsNormals DarkNormalsVertex(DarkAttributesNormals v)
{
    DarkVaryingsNormals o;
    UNITY_SETUP_INSTANCE_ID(v);
    UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

    o.positionCS = TransformObjectToHClip(v.positionOS);
    o.uv = TRANSFORM_TEX(v.uv, _NormalMap);
    o.color = v.color;
    o.normalWS = -GetViewForwardDir();
    o.tangentWS = TransformObjectToWorldDir(v.tangent.xyz);
    o.bitangentWS = cross(o.normalWS, o.tangentWS) * v.tangent.w;
    return o;
}

half4 DarkNormalsFragment(DarkVaryingsNormals i) : SV_Target
{
    half4 mainTex = i.color * SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv);
    half3 normalTS = UnpackNormal(SAMPLE_TEXTURE2D(_NormalMap, sampler_NormalMap, i.uv));
    return NormalsRenderingShared(mainTex, normalTS, i.tangentWS, i.bitangentWS, i.normalWS);
}


#endif