Shader "Custom/ShadowShader"
{
    Properties
    {
        [PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
        _ShadowColor ("Shadow Color", Color) = (0, 0, 0, 0.5)
        _SunDirection ("Sun Direction", Float) = 0
        _PerspectiveStrength ("_PerspectiveStrength", Float) = 0.5
        _ShadowScale ("Scale", Vector) = (0.5, 0.5, 0)
        [Header(Not Changed)]
        _RootPos  ("Root Position", Vector) = (0, 0, 0)
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
            }
        LOD 100
        Cull Off
        Lighting Off
        Ztest Less
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                uint id: SV_VertexID;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
            };

            sampler2D _MainTex;

            float _PerspectiveStrength;
            float3 _ShadowScale;
            float4 _RootPos;

            float4 _ShadowColor;
            float _SunDirection;
            v2f vert(appdata_t v)
            {
                float4 shifted = v.vertex;
                shifted.xyz = ((shifted.xyz - _RootPos.xyz)*_ShadowScale)+_RootPos.xyz;
                
                float3 worldPos = mul(unity_ObjectToWorld, shifted).xyz;

                float3 offset = shifted.xyz  - _RootPos.xyz;
                

                //_PerspectiveStrengthworldPos.y *= _ShadowScale.y; //fucks up vertices

                // --- Shear transformation ---
                offset.x += offset.y * (_SunDirection * _PerspectiveStrength);

                // --- Perspective scaling ---
                float perspective = offset.y * _PerspectiveStrength;
                offset *= perspective;

                worldPos += offset;


                v2f o;
                o.vertex = UnityWorldToClipPos(float4(worldPos, 1.0));
                o.uv = v.uv;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float4 color = tex2D(_MainTex, i.uv);
                return color.a * _ShadowColor; 
            }
            ENDCG
        }
    }
}
