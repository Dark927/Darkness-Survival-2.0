Shader "Custom/2DSilhouetteFront"
{
    Properties
    {
        _MainTex ("Base Texture", 2D) = "white" { }
        _OutlineColor ("Outline Color", Color) = (0,0,0,1)
        _OutlineWidth ("Outline Width", Float) = 0.1
    }
    
    SubShader
    {
        Tags { "RenderType"="Opaque" }

        Pass
        {
            Name"OUTLINE"
            Tags { "LightMode" = "Always" }

            ZWrite On
            ZTest LEqual
            Cull Front
            ColorMask RGB

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 pos : POSITION;
                float3 normal : TEXCOORD0;
            };

            // Outline color and width
            float _OutlineWidth;
            fixed4 _OutlineColor;

            v2f vert(appdata_t v)
            {
                v2f o;
                // Modify the vertex position to create an outline effect
                o.pos = UnityObjectToClipPos(v.vertex + v.normal * _OutlineWidth);
                o.normal = v.normal;
                return o;
            }

            half4 frag(v2f i) : SV_Target
            {
                return _OutlineColor; // Return the outline color
            }

            ENDCG
        }

    }

    Fallback "Diffuse"
}
