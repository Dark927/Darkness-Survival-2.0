
Shader "Dark/UI/ImageGlowSDF"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1, 1, 1, 1)

        _StencilComp ("Stencil Comparison", Float) = 8
        _Stencil ("Stencil ID", Float) = 0
        _StencilOp ("Stencil Operation", Float) = 0
        _StencilWriteMask ("Stencil Write Mask", Float) = 255
        _StencilReadMask ("Stencil Read Mask", Float) = 255
        _ColorMask ("Color Mask", Float) = 15
        [Toggle(UNITY_UI_ALPHACLIP)] _UseUIAlphaClip ("Use Alpha Clip", Float) = 0


        [Header(SDF)]
        [Space]
        [HDR]_SDFColor ("SDF Color", Color) = (1,1,1,1)
        _SDFFigure ("Figure", Integer) = 1
        _SDFScale ("Effect Scale", Range(1,3)) = 1.5
        _SDFCenterSize ("Center / Size", Vector) = (0.5, 0.5, 0.2, 0.2)
        _SDFGamma ("Gamma", Range(0,2)) = 0.2
        _SDFMultiplier ("Multiplier", Float) = 1.9

    }

    SubShader
    {
        Tags
        {
            "Queue" = "Transparent"
            "IgnoreProjector" = "True"
            "RenderType" = "Transparent"
            "PreviewType" = "Plane"
            "CanUseSpriteAtlas" = "True"
            "DisableBatching" = "True"
        }

        Stencil
        {
            Ref [_Stencil]
            Comp [_StencilComp]
            Pass [_StencilOp]
            ReadMask [_StencilReadMask]
            WriteMask [_StencilWriteMask]
        }

        Cull Off
        Lighting Off
        ZWrite Off
        ZTest [unity_GUIZTestMode]
        Blend SrcAlpha OneMinusSrcAlpha
        ColorMask [_ColorMask]

        Pass
        {
            Name "Default"
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 2.0

            #include "UnityCG.cginc"
            #include "UnityUI.cginc"
            #include "/Assets/Materials/Shaders/Common/DarkFuncs.hlsl"

            #pragma multi_compile_local _ UNITY_UI_CLIP_RECT
            #pragma multi_compile_local _ UNITY_UI_ALPHACLIP

            struct appdata_t
            {
                float4 vertex : POSITION;
                float4 color : COLOR;
                float2 texcoord : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                fixed4 color : COLOR;
                float2 texcoord : TEXCOORD0;
                float4 worldPosition : TEXCOORD1;
                float2 original : TEXCOORD2;
                UNITY_VERTEX_OUTPUT_STEREO
            };

            sampler2D _MainTex;
            fixed4 _Color;
            fixed4 _TextureSampleAdd;
            float4 _ClipRect;
            float4 _MainTex_ST;
            //
            float4 _SDFColor;
            int _SDFFigure;
            float4 _SDFCenterSize;
            float _SDFScale;
            float _SDFGamma;
            float _SDFMultiplier;

            v2f vert(appdata_t v)
            {
                v2f OUT;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);  
                OUT.worldPosition = v.vertex;
                OUT.vertex = UnityObjectToClipPos(OUT.worldPosition);
                OUT.original = TRANSFORM_TEX(v.texcoord, _MainTex);

                // Apply scaling to UVs adding padding
                float2 centerUV = float2(0.5, 0.5); // Assuming UVs are centered at (0.5, 0.5)
                OUT.texcoord = (OUT.original - centerUV) * _SDFScale + centerUV;

                OUT.color = v.color * _Color;
                return OUT;
            }

            fixed4 frag(v2f IN) : SV_Target
            {
                half4 color = (tex2D(_MainTex, IN.texcoord) + _TextureSampleAdd) * IN.color;

                #ifdef UNITY_UI_CLIP_RECT
                color.a *= UnityGet2DClipping(IN.worldPosition.xy, _ClipRect);
                #endif

                #ifdef UNITY_UI_ALPHACLIP
                clip (color.a - 0.001);
                #endif

                float2 p = IN.original - _SDFCenterSize.xy;
                float blur;

                switch(_SDFFigure)
                {
                    case 0:
                        blur = sdCircle(p, _SDFCenterSize.zw);
                        break;
                    case 1:
                        blur = sdBox(p, _SDFCenterSize.zw);
                        break;
                    case 2:
                        blur = sdRhombus(p, _SDFCenterSize.zw);
                        break;
                    default:
                        blur = 0;
                        break;
                }

                blur = saturate(blur); //prevent negative powers
                blur = pow(blur, _SDFGamma);
                blur = 1 - blur * _SDFMultiplier;
                blur = saturate(blur); //prevent negative powers

                float4 colorGauss = _SDFColor * blur;
                //return colorGauss;

                color = lerp(color, colorGauss, 1 - color.a);
                //return float4(blur.rrr, 1);

                return color;
            }
            ENDCG
        }
    }
}