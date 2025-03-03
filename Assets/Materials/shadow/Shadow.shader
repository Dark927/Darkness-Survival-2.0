Shader "Custom/AnimatedShadowShader"
{
    Properties
    {
        [PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
        _ShadowColorDay ("Day Shadow Color", Color) = (0.1, 0.1, 0.1, 0.5)
        _ShadowColorSunset ("Sunset Shadow Color", Color) = (0.4, 0.2, 0.1, 0.3)
        _SunDirection ("Sun Base Direction", Float) = 0
        _PerspectiveStrength ("Perspective Strength", Float) = 0.5
        _ShadowScale ("Scale", Vector) = (0.5, 0.5, 0)
        _TimeFactor ("Time", Float) = 1.0
        _RootPos  ("Root Position", Vector) = (0, 0, 0, 1)
    }
    SubShader
    {
        Tags { "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" }
        LOD 100
        Cull Off
        Lighting Off
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
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
                float4 shadowColor : COLOR;
            };

            sampler2D _MainTex;
            float4 _ShadowColorDay, _ShadowColorSunset;
            float _SunDirection, _PerspectiveStrength, _TimeFactor;
            float3 _ShadowScale;
            float4 _RootPos;
            static const float PI2 = 6.283185307;
            v2f vert(appdata_t v)
            {
                float timeFactor = _TimeFactor; // Animate based on game time
                float sunAngle = _SunDirection + sin(timeFactor * PI2) * 45; // Oscillating sun direction

                // Interpolate sun height (simulates daytime transition)
                float sunElevation = 0.5 + 0.5 * cos(timeFactor * PI2); // 0 at noon, 1 at sunrise/sunset
                float3 shadowOffset = (v.vertex.xyz - _RootPos.xyz) * _ShadowScale;
                
                // Fake 3D perspective
                shadowOffset.x += shadowOffset.y * (sunAngle * _PerspectiveStrength);
                shadowOffset *= sunElevation; // Shadows longer at lower sun elevation

                float3 worldPos = mul(unity_ObjectToWorld, float4(v.vertex.xyz, 1.0)).xyz;
                worldPos += shadowOffset;

                v2f o;
                o.vertex = UnityWorldToClipPos(float4(worldPos, 1.0));
                o.uv = v.uv;

                // Color interpolation between day and sunset
                float sunsetFactor = saturate(abs(sunElevation - 0.5) * 2);
                o.shadowColor = lerp(_ShadowColorDay, _ShadowColorSunset, sunsetFactor);
                
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float4 texColor = tex2D(_MainTex, i.uv);
                return i.shadowColor * texColor;
            }
            ENDCG
        }
    }
}
