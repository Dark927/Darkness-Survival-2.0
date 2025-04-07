Shader "Dark/Sprites/DarkEnemyFX"
{
    Properties
    {
        _MainTex("Diffuse", 2D) = "white" {}
        _MaskTex("Mask", 2D) = "white" {}
        _ColorMaskTex("Color Mask", 2D) = "white" {}
        _EmissionMaskTex("Emission Mask", 2D) = "black" {}
        _NormalMap("Normal Map", 2D) = "bump" {}

        [Header(Mask Tint)]
        [Gamma] _Gamma("Gamma", Range(0.0, 3)) = 1
        _Color("Tint", Color) = (1,1,1,1)
        [Space]
        [Header(Flash)]
        _FlashColor ("Flash Color", Color) = (1,1,1,1)
		_FlashAmount ("Flash Amount", Range (0,1)) = 0

        [Space]
        [Header(Emission)]
        [HDR]
        _EmissionColor ("Emission Color", Color) = (1,1,1,1)
		_EmissionAmount ("Emission Amount", Range (0,1)) = 0

        [Header(Dissolve)]
        _Seed ("Random Seed", Range (0,1)) = 0.42
        _DissolveNoise ("Dissolve Noise", Range (0,5000)) = 1000
        [HDR]
        _DissolveColor ("Dissolve Color", Color) = (1,1,1,1)
        _DissolveOutline ("Dissolve Outline", Range (1,2)) = 1.1
        _DissolveAmount ("Dissolve Amount", Range (0,1)) = 0

        _HsvaB ("(Hb Sb Vb Ab)", Vector) = (0,0,0,0)

        [Space]
        [Header(HSV Replace)]
        [Space]
        _TargetColor ("Target Color", Color) = (1,0,0,1)

        [Header(Tolerances HSV)]
        [Space]
        _HueTolerance ("Hue Tolerance", Range(0, 0.5)) = 0.1
        _SatTolerance ("Saturation Tolerance", Range(0, 1)) = 0.2
        _ValTolerance ("Value Tolerance", Range(0, 1)) = 0.2

        [Space]
        _HSVGamma ("HSV Gamma", Range(-1, 5)) = 1

        [Space]
        [Header(WARNING. Global for all sprites. Use only for debug)]
        _FXFeatures ("FX Features", Integer) = 0
        _RendMode ("Renderer Mode", Integer) = 0


        // Legacy properties. They're here so that materials using this shader can gracefully fallback to the legacy sprite shader.
        [HideInInspector] _RendererColor("RendererColor", Color) = (1,1,1,1)
        [HideInInspector] _Flip("Flip", Vector) = (1,1,1,1)
        [HideInInspector] _AlphaTex("External Alpha", 2D) = "white" {}
        [HideInInspector] _EnableExternalAlpha("Enable External Alpha", Float) = 0
    }

    SubShader
    {
        Tags {"Queue" = "Transparent" "RenderType" = "Transparent" "RenderPipeline" = "UniversalPipeline" }

        Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
        Cull Off
        ZWrite Off

        Pass
        {
            Tags { "LightMode" = "Universal2D" }
            HLSLPROGRAM
            
            #pragma vertex DarkVertex
            #pragma fragment Universal2DFragment
            #pragma multi_compile USE_SHAPE_LIGHT_TYPE_0 __
            #pragma multi_compile USE_SHAPE_LIGHT_TYPE_1 __
            #pragma multi_compile USE_SHAPE_LIGHT_TYPE_2 __
            #pragma multi_compile USE_SHAPE_LIGHT_TYPE_3 __
            #pragma multi_compile _ DEBUG_DISPLAY
            
            #include "Common/DarkCommonLit.hlsl"
            #include "Common/DarkFX.hlsl"

            TEXTURE2D(_ColorMaskTex);//TODO: remove
            SAMPLER(sampler_ColorMaskTex);

            
            float4 Universal2DFragment(DarkVaryings2D i) : SV_Target
            {
                float2 smoothUV;
                
                [branch]
                if (_FXFeatures & DARKFX_JITTERFREE)
                { 
                    smoothUV = TexturePointSmoothUV(i.uv);
                }
                else
                {
                    smoothUV = i.uv;
                }

                float4 main = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, smoothUV);

                [branch]
                if(_RendMode == RENDMODE_UNLIT)
                    return main;

                ApplyDarkFX(main, i.color, smoothUV, i.uv);

                SurfaceData2D surfaceData;
                InputData2D inputData;
                InitializeSurfaceData(main.rgb, main.a, SAMPLE_TEXTURE2D(_MaskTex, sampler_MaskTex, smoothUV), surfaceData);
                InitializeInputData(smoothUV, i.lightingUV, inputData);
                return CombinedShapeLightShared(surfaceData, inputData);
            }
            ENDHLSL
        }

        Pass
        {
            Tags { "LightMode" = "NormalsRendering" }

            HLSLPROGRAM
            #pragma vertex DarkNormalsVertex
            #pragma fragment DarkNormalsFragment
            #include "Common/DarkCommonLit.hlsl"
            ENDHLSL
        }

        Pass
        {
            Tags { "LightMode" = "UniversalForward" "Queue"="Transparent" "RenderType"="Transparent"}

            HLSLPROGRAM
            #pragma vertex DarkVertex
            #pragma fragment DarkUnlitFragment
            #include "Common/DarkCommonLit.hlsl"
            ENDHLSL
        }
      
    }
    Fallback "Sprites/Default"
}