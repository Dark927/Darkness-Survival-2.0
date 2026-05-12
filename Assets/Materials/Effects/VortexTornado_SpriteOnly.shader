Shader "Custom/2D/VortexTornado_SpriteOnly"
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
        
        [Space(10)]
        [Header(Turbulence Sine Wave)]
        _NoiseStrength ("Turbulence Strength", Range(0.0, 0.1)) = 0.02
        _NoiseScale ("Turbulence Scale", Float) = 15.0
        _NoiseSpeed ("Turbulence Speed", Float) = 3.0
        
        [Space(10)]
        [Header(Fast Texture Noise Warp)]
        [NoScaleOffset] _NoiseTex ("Noise Texture", 2D) = "grey" {}
        _WarpStrength ("Texture Warp Strength", Range(0.0, 0.2)) = 0.05
        _WarpScale ("Texture Warp Scale", Float) = 2.0
        _WarpScroll ("Scroll Speed X and Y", Vector) = (0.5, 0.5, 0, 0)
        
        [Space(10)]
        [Header(Sprite Visibility Mask)]
        _MaskRadius ("Sprite Mask Radius", Range(0.0, 0.7)) = 0.45
        _MaskSoftness ("Sprite Mask Softness", Range(0.001, 0.5)) = 0.2
    }
    
    SubShader
    {
        Tags 
        { 
            "RenderType"="Transparent" 
            "Queue"="Transparent" 
            "RenderPipeline"="UniversalPipeline"
        }

        // Standard Alpha Blending
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
                float4 color        : COLOR;
            };

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);

            TEXTURE2D(_NoiseTex);
            SAMPLER(sampler_NoiseTex);

            CBUFFER_START(UnityPerMaterial)
                float4 _Color;
                float _TwistAmount;
                float _RotationSpeed;
                float _Radius;
                
                float _NoiseStrength;
                float _NoiseScale;
                float _NoiseSpeed;
                
                float _WarpStrength;
                float _WarpScale;
                float4 _WarpScroll;
                
                float _MaskRadius;
                float _MaskSoftness;
            CBUFFER_END

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.uv = IN.uv;
                // Pass standard vertex colors (so SpriteRenderer tinting still works)
                OUT.color = IN.color; 
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

                // 2. FAST TEXTURE NOISE (Optimized Warp)
                float2 noiseUV = (IN.uv * _WarpScale) + (_Time.y * _WarpScroll.xy);
                half4 noiseColor = SAMPLE_TEXTURE2D(_NoiseTex, sampler_NoiseTex, noiseUV);
                float2 warpOffset = (noiseColor.rg - 0.5) * _WarpStrength;

                // Combine offsets
                float2 delta = pureDelta + noiseOffset + warpOffset;
                float radius = length(delta);
                
                // 3. VORTEX TWIST MATH
                float falloff = smoothstep(_Radius, 0.0, radius);
                float angle = atan2(delta.y, delta.x);
                angle += (_TwistAmount * falloff) + (_Time.y * _RotationSpeed);

                float2 swirledUV = center + float2(cos(angle), sin(angle)) * radius;

                // 4. SPRITE SAMPLING
                half4 spriteColor = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, swirledUV) * _Color * IN.color;

                // 5. MASKING
                float spriteMask = smoothstep(_MaskRadius, _MaskRadius - _MaskSoftness, pureRadius);
                spriteColor.a *= spriteMask; 

                // Softly clip the extreme edges to prevent harsh quad lines
                float quadAlpha = smoothstep(0.5, 0.48, pureRadius);
                spriteColor.a *= quadAlpha;

                return spriteColor;
            }
            ENDHLSL
        }
    }
}