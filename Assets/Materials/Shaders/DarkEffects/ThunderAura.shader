Shader "Dark/VFX/ThunderAura"
{
    Properties
    {
        [HDR] _CoreColor ("Core Color", Color) = (0.8, 1, 1, 1)
        [HDR] _EdgeColor ("Edge Color", Color) = (0, 0.5, 1, 0)
        _Speed ("Storm Speed", Float) = 4.0
        _SpikeCount ("Lightning Branches", Float) = 12.0
        _Sharpness ("Lightning Sharpness", Float) = 30.0
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
            #include "UnityCG.cginc"

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

            float4 _CoreColor;
            float4 _EdgeColor;
            float _Speed;
            float _SpikeCount;
            float _Sharpness;
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
                
                // Distance and Angle
                float dist = length(uv) * 2.0;
                float angle = atan2(uv.y, uv.x);
                float time = _Time.y * _Speed;

                // 1. THE LIGHTNING MATH
                // By taking the absolute inverse of a sine wave, we get perfectly sharp points.
                float wave1 = 1.0 - abs(sin(angle * _SpikeCount + time));
                float wave2 = 1.0 - abs(sin(angle * (_SpikeCount * 0.7) - time * 1.4));
                float wave3 = 1.0 - abs(sin(angle * (_SpikeCount * 1.3) + time * 0.8));

                // Combine them to create chaotic, erratic branches
                float lightning = wave1 * wave2 * wave3;
                
                // Raise to a high power to make the beams razor-thin
                lightning = pow(max(0, lightning), _Sharpness * 0.1);

                // 2. THE INNER CRACKLE
                // Add high-frequency noise near the center so the core looks unstable
                float crackle = 1.0 - abs(sin(angle * 30.0 - time * 2.5));
                crackle = pow(max(0, crackle), _Sharpness * 0.2);

                // 3. COMBINE & MASK
                // Add the lightning and crackle, but fade the crackle out before it reaches the edge
                float energy = lightning + (crackle * 0.5 * smoothstep(0.8, 0.1, dist));
                
                // Fade everything out at the physical edges of the quad so it doesn't clip
                float sphereMask = smoothstep(1.0, 0.2, dist);
                energy *= sphereMask;

                // 4. PULSE
                float pulse = lerp(1.0, (sin(time * 3.0) * 0.5 + 0.5), _Pulse);

                // 5. COLOR BLENDING
                fixed4 finalColor = lerp(_CoreColor, _EdgeColor, dist * 1.2);
                
                // Boost the color massively where the lightning is striking
                finalColor.rgb += _CoreColor.rgb * energy * 3.0;

                // 6. ALPHA
                // Make the background mostly transparent, only showing the lightning and a faint core glow
                fixed coreGlow = smoothstep(0.4, 0.0, dist) * 0.2;
                fixed finalAlpha = (energy + coreGlow) * pulse * finalColor.a;

                // Output final HDR color
                return fixed4(finalColor.rgb * pulse, saturate(finalAlpha));
            }
            ENDCG
        }
    }
}