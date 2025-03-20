Shader "Sprites/DarkEnemyFX"
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
        [HDR]
        _Seed ("Random Seed", Range (0,1)) = 0
        _DissolveAmount ("Dissolve Amount", Range (0,1)) = 0

        //

        [Space]
        [Header(HSV Replace)]
        [Space]
        _TargetColor ("Target Color", Color) = (1,0,0,1)

        [Space]
        _ReplacementColorCoefK ("(Hk Sk Vk)", Vector) = (1,1,1,0)
        _ReplacementColorCoefB ("(Hb Sb Vb)", Vector) = (0,0,0,0)

        [Header(Tolerances HSV)]
        [Space]
        _HueTolerance ("Hue Tolerance", Range(0, 0.5)) = 0.1
        _SatTolerance ("Saturation Tolerance", Range(0, 1)) = 0.2
        _ValTolerance ("Value Tolerance", Range(0, 1)) = 0.2

        [Space]
        _HSVGamma ("HSV Gamma", Range(-1, 5)) = 1

        [Space]
        [Header(WARNING. Global for all sprites. Use only for debug)]
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

            TEXTURE2D(_ColorMaskTex);
            SAMPLER(sampler_ColorMaskTex);

            TEXTURE2D(_EmissionMaskTex);
            SAMPLER(sampler_EmissionMaskTex);

            float4 _FlashColor;
            float4 _EmissionColor;
            float _EmissionAmount;
            float _Gamma;
            float _FlashAmount;

            float _Seed;
            float _DissolveAmount;

            float4 _TargetColor;
            float4 _ReplacementColorCoefK;
            float4 _ReplacementColorCoefB;
            float _HueTolerance;
            float _SatTolerance;
            float _ValTolerance;
            float _HSVGamma;

            int _RendMode;


            float4 ProcessHSV(float4 col)
            {
                float3 hsv = rgb_to_hsv(col.rgb);
                float3 targetHSV = rgb_to_hsv(_TargetColor.rgb); //TODO: move to c#

                // Calculate hue difference with wrap-around
                float hueDiff = abs(hsv.x - targetHSV.x);
                hueDiff = min(hueDiff, 1.0 - hueDiff);

                // Create replacement color version
                float3 replacedHsv = hsv;                
                replacedHsv *= _ReplacementColorCoefK.xyz;
                replacedHsv += _ReplacementColorCoefB.xyz;
                float3 replacedRGB = hsv_to_rgb(replacedHsv);
                
                // STEP or MASKSTEP
                if(_RendMode == 1 || _RendMode == 3){

                    if (hueDiff <= _HueTolerance &&
                        abs(hsv.y - targetHSV.y) <= _SatTolerance &&
                        abs(hsv.z - targetHSV.z) <= _ValTolerance)
                    { 
                        //set mask
                        if(_RendMode == 3) 
                            replacedRGB.rgb = float3(1,0,1);

                        return float4(replacedRGB.rgb, col.a);
                    }
                    return col;
                }
                else //INTERP or MASKINTERP
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

                    // MASKINTERP
                    if(_RendMode == 4)
                        return float4(totalBlend.rrr, col.a);
                    else
                        return float4(interpolatedRGB, col.a);
                }
            }
            float hash21(float2 p)
            {
                p = frac(p * float2(123.34, 456.21));
                p += dot(p, p + 45.32);
                return frac(p.x * p.y);
            }
            
            float4 Universal2DFragment(DarkVaryings2D i) : SV_Target
            {
                float2 smoothUV = TexturePointSmoothUV(i.uv);
                float colormaskCoef = SAMPLE_TEXTURE2D(_ColorMaskTex, sampler_ColorMaskTex, smoothUV).r; //TODO: remove
                float4 main = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, smoothUV);

                if(_RendMode == 5) // unlit
                    return main;

                if(_RendMode != 0) // not original
                    main = ProcessHSV(main);

                if(_RendMode > 2) // masks
                    return main;

                float3 tinted = pow(abs(main.rgb * i.color.rgb), _Gamma);
                main.rgb = lerp(main.rgb, tinted, colormaskCoef);
                main.rgb = lerp(main.rgb, _FlashColor.rgb, _FlashAmount);

                float emissionCoef = SAMPLE_TEXTURE2D(_EmissionMaskTex, sampler_EmissionMaskTex, smoothUV).r;
                main.rgb += _EmissionColor.rgb * emissionCoef * _EmissionAmount;

                float noiseTex = pow(noise(smoothUV * 1000, _Seed), 0.5); //todo: move 10000 to settings
                float tt = step(noiseTex , _DissolveAmount);
                float tt2 = step(noiseTex , _DissolveAmount * 1.1);
                float dd = tt2-tt;

                main.rgb = lerp(main.rgb,float3(0,1,1),dd);
                main.a = saturate(main.a * (1-tt));
                

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