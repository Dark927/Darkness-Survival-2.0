Shader "Dark/VFX/EnergySphere"
{
    Properties
    {
        [HDR] _CoreColor ("Core Color", Color) = (0, 1, 1, 1)
        [HDR] _EdgeColor ("Edge Color", Color) = (0, 0, 1, 0)
        _Speed ("Animation Speed", Float) = 3.0
        _Density ("Energy Density", Float) = 10.0
        _Pulse ("Pulse Amount", Range(0, 1)) = 0.5
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" "IgnoreProjector"="True" }
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        Cull Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc" // The standard, bulletproof Unity library

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            // Variables
            float4 _CoreColor;
            float4 _EdgeColor;
            float _Speed;
            float _Density;
            float _Pulse;

            v2f vert(appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex); 
                o.uv = v.uv;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                // Center the UVs (-0.5 to 0.5)
                float2 uv = i.uv - 0.5;

                // Distance from center
                float dist = length(uv) * 2.0;

                // Circular Mask
                float sphereMask = 1.0 - smoothstep(0.9, 1.0, dist);

                // Angle for swirling math
                float angle = atan2(uv.y, uv.x);

                // Procedural Energy Rings
                float time = _Time.y * _Speed;
                float energy = sin(dist * _Density - time) + sin(angle * 4.0 + time);
                energy = smoothstep(0.0, 1.5, energy); // Soften the energy bands

                // Pulse effect
                float pulseMultiplier = lerp(1.0, (sin(time * 2.0) * 0.5 + 0.5), _Pulse);

                // Color blending
                fixed4 finalColor = lerp(_CoreColor, _EdgeColor, dist);

                // Add energy highlights
                finalColor.rgb += _CoreColor.rgb * energy * 0.5;

                // Final Alpha
                fixed finalAlpha = sphereMask * finalColor.a * (0.5 + energy * 0.5) * pulseMultiplier;

                // Output RGB multiplied by pulse for HDR intensity
                return fixed4(finalColor.rgb * pulseMultiplier, finalAlpha);
            }
            ENDCG
        }
    }
}