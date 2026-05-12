Shader "Custom/2D/VortexTornado"
{
    Properties
    {
        [HideInInspector] _MainTex ("Sprite Texture", 2D) = "white" {}
        [HDR] _Color ("Tornado Tint and Alpha", Color) = (1, 1, 1, 0.5)
        
        [Space(10)]
        [Header(Vortex Core Settings)]
        _TwistAmount ("Twist Amount", Float) = 15.0
        _RotationSpeed ("Rotation Speed", Float) = -5.0
        _Radius ("Distortion Radius", Range(0.01, 0.7)) = 0.5
        _BgDistortion ("Background Distortion Strength", Range(0.0, 2.0)) = 1.0
        
        [Space(10)]
        [Header(Turbulence Sine Wave)]
        _NoiseStrength ("Turbulence Strength", Range(0.0, 0.1)) = 0.02
        _NoiseScale ("Turbulence Scale", Float) = 15.0
        _NoiseSpeed ("Turbulence Speed", Float) = 3.0
        
        [Space(10)]
        [Header(Extra Warp Voronoi Noise)]
        _WarpStrength ("Warp Strength", Range(0.0, 0.2)) = 0.03
        _WarpScale ("Warp Cell Scale", Float) = 8.0
        _WarpSpeed ("Warp Boil Speed", Float) = 5.0
        
        [Space(10)]
        [Header(Sprite Visibility Mask)]
        _MaskRadius ("Sprite Mask Radius", Range(0.0, 0.7)) = 0.45
        _MaskSoftness ("Sprite Mask Softness", Range(0.001, 0.5)) = 0.2

        [Space(10)]
        [Header(Background Distortion Mask and Tint)]
        _BgMaskRadius ("BG Mask Radius", Range(0.0, 0.7)) = 0.5
        _BgMaskSoftness ("BG Mask Softness", Range(0.001, 0.5)) = 0.2
        [HDR] _BgTintColor ("BG Tint RGB and Strength Alpha", Color) = (1, 1, 1, 0.0)
    }
    
    SubShader
    {
        Tags 
        { 
            "RenderType"="Transparent" 
            "Queue"="Transparent" 
            "RenderPipeline"="UniversalPipeline"
        }

        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        Cull Off

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS   : POSITION;
                float2 uv           : TEXCOORD0;
                float4 color        : COLOR;
            };

            struct Varyings
            {
                float4 positionCS   : SV_POSITION;
                float2 uv           : TEXCOORD0;
                float4 screenPos    : TEXCOORD1;
                float4 color        : COLOR;
            };

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);

            TEXTURE2D(_CameraSortingLayerTexture);
            SAMPLER(sampler_CameraSortingLayerTexture);

            CBUFFER_START(UnityPerMaterial)
                float4 _Color;
                float _TwistAmount;
                float _RotationSpeed;
                float _Radius;
                float _BgDistortion;
                
                float _NoiseStrength;
                float _NoiseScale;
                float _NoiseSpeed;
                
                float _WarpStrength;
                float _WarpScale;
                float _WarpSpeed;
                
                float _MaskRadius;
                float _MaskSoftness;

                float _BgMaskRadius;
                float _BgMaskSoftness;
                float4 _BgTintColor;
            CBUFFER_END

            // --- PROCEDURAL VORONOI NOISE FUNCTIONS ---
            float2 VoronoiRandomVector(float2 UV, float offset)
            {
                float2 rnd = float2(
                    frac(sin(dot(UV, float2(12.9898, 78.233))) * 43758.5453),
                    frac(sin(dot(UV, float2(39.346, 11.135))) * 43758.5453)
                );
                return float2(sin(rnd.y * offset) * 0.5 + 0.5, cos(rnd.x * offset) * 0.5 + 0.5);
            }

            float Voronoi(float2 UV, float AngleOffset, float CellDensity)
            {
                float2 g = floor(UV * CellDensity);
                float2 f = frac(UV * CellDensity);
                float res = 8.0;

                for (int y = -1; y <= 1; y++)
                {
                    for (int x = -1; x <= 1; x++)
                    {
                        float2 lattice = float2(x, y);
                        float2 offset = VoronoiRandomVector(lattice + g, AngleOffset);
                        float d = distance(lattice + offset, f);
                        res = min(res, d);
                    }
                }
                return res;
            }
            // ------------------------------------------

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.uv = IN.uv;
                OUT.color = IN.color;
                OUT.screenPos = ComputeScreenPos(OUT.positionCS);
                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                float2 center = float2(0.5, 0.5);
                float2 pureDelta = IN.uv - center;
                float pureRadius = length(pureDelta);

                // 1. BASIC TURBULENCE (Sine Wave)
                float nt = _Time.y * _NoiseSpeed;
                float2 noiseOffset = float2(
                    sin(IN.uv.y * _NoiseScale + nt) + cos(IN.uv.x * _NoiseScale * 0.7 - nt),
                    cos(IN.uv.x * _NoiseScale + nt) + sin(IN.uv.y * _NoiseScale * 0.8 - nt)
                ) * _NoiseStrength;

                // 2. EXTRA WARP (Voronoi / Cellular Noise)
                float vTime = _Time.y * _WarpSpeed;
                float warpX = Voronoi(IN.uv, vTime, _WarpScale);
                float warpY = Voronoi(IN.uv + float2(12.4, 5.3), vTime, _WarpScale);
                float2 warpOffset = float2(warpX - 0.5, warpY - 0.5) * _WarpStrength;

                // Combine all offsets
                float2 delta = pureDelta + noiseOffset + warpOffset;
                float radius = length(delta);
                
                // 3. VORTEX TWIST MATH
                float falloff = smoothstep(_Radius, 0.0, radius);
                float angle = atan2(delta.y, delta.x);
                angle += (_TwistAmount * falloff) + (_Time.y * _RotationSpeed);

                float2 swirledUV = center + float2(cos(angle), sin(angle)) * radius;

                // 4. BACKGROUND DISTORTION MASKING & SAMPLING
                float2 uvOffset = swirledUV - IN.uv;
                float bgMask = smoothstep(_BgMaskRadius, _BgMaskRadius - _BgMaskSoftness, pureRadius);
                uvOffset *= bgMask;

                float2 screenUV = IN.screenPos.xy / IN.screenPos.w;
                float2 spriteToScreenRatio = float2(
                    length(ddx(screenUV)) / (length(ddx(IN.uv)) + 1e-5),
                    length(ddy(screenUV)) / (length(ddy(IN.uv)) + 1e-5)
                );

                float2 distortedScreenUV = screenUV + (uvOffset * spriteToScreenRatio * _BgDistortion);
                half4 bgColor = SAMPLE_TEXTURE2D(_CameraSortingLayerTexture, sampler_CameraSortingLayerTexture, distortedScreenUV);

                // Apply the Background Tint using the bgMask
                bgColor.rgb = lerp(bgColor.rgb, bgColor.rgb * _BgTintColor.rgb, _BgTintColor.a * bgMask);

                // 5. SPRITE MASKING & SAMPLING
                float spriteMask = smoothstep(_MaskRadius, _MaskRadius - _MaskSoftness, pureRadius);
                half4 spriteColor = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, swirledUV) * _Color * IN.color;
                spriteColor.a *= spriteMask; 

                // 6. BLENDING
                half3 finalRGB = lerp(bgColor.rgb, spriteColor.rgb, spriteColor.a);
                
                // Ensure the very edges of the mesh bounds smoothly clip off to avoid hard quad artifacts
                float quadAlpha = smoothstep(0.5, 0.48, pureRadius);

                return half4(finalRGB, quadAlpha);
            }
            ENDHLSL
        }
    }
}