Shader "Custom/Shred"
{
    Properties {
        _MainTex ("Sprite Texture", 2D) = "white" {}
        _EnemyID ("Enemy ID", Float) = 0
        _TimeScale ("Time Scale", Float) = 1.0
        _ShredSpeed ("Shred Speed", Float) = 1.0
    }
    SubShader {
        Tags { "RenderType"="Opaque" "Queue"="Geometry" }
        LOD 300

        Pass {
            Name "TessellationAndGeometry"
            CGPROGRAM
            // Enable tessellation and geometry shader stages; target 5.0+ required.
            #pragma vertex vert
            #pragma hull hs_main
            #pragma domain domain
            #pragma geometry geom
            #pragma fragment frag
            #pragma target 5.0

            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float _EnemyID;
            float _TimeScale;
            float _ShredSpeed;

            // Input structure from the mesh.
            struct appdata {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            // Data passed between stages.
            struct v2f {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
                float4 worldPos : TEXCOORD1;
            };

            // Vertex shader: transform vertices and pass along UVs.
            v2f vert(appdata v) {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.worldPos = mul(unity_ObjectToWorld, v.vertex);
                return o;
            }

            // ---------------------
            // Tesselation (Hull) Shader
            // ---------------------
            struct HS_ControlPoint {
                float4 pos : POSITION;
                float2 uv : TEXCOORD0;
                float4 worldPos : TEXCOORD1;
            };

            // Each output control point comes directly from the vertex shader output.
            [domain("tri")]
            [partitioning("fractional_odd")]
            [outputtopology("triangle_cw")]
            [outputcontrolpoints(3)]
            [patchconstantfunc("HSConstants")]
            HS_ControlPoint hs_main(InputPatch<v2f,3> patch, uint i : SV_OutputControlPointID)
            {
                HS_ControlPoint cp;
                cp.pos = patch[i].pos;
                cp.uv = patch[i].uv;
                cp.worldPos = patch[i].worldPos;
                return cp;
            }

            // Set tessellation factors (you can modulate these based on screen distance etc.)
            struct HS_PatchConstants {
                float TessFactor[3] : SV_TessFactor;
                float InsideTessFactor : SV_InsideTessFactor;
            };

            HS_PatchConstants HSConstants(InputPatch<v2f,3> patch)
            {
                HS_PatchConstants constants;
                float tess = 32.0; // Increase for finer splitting.
                constants.TessFactor[0] = tess;
                constants.TessFactor[1] = tess;
                constants.TessFactor[2] = tess;
                constants.InsideTessFactor = tess;
                return constants;
            }

            // ---------------------
            // Domain Shader
            // ---------------------
            [domain("tri")]
            [partitioning("fractional_odd")]
            [outputtopology("triangle_cw")]
            [outputcontrolpoints(3)]
            [patchconstantfunc("HSConstants")]
            v2f domain(HS_PatchConstants pc, const OutputPatch<HS_ControlPoint, 3> patch, float3 bary : SV_DomainLocation)
            {
                v2f o;
                // Barycentric interpolation of clip space position, UV and world position.
                float4 pos = patch[0].pos * bary.x + patch[1].pos * bary.y + patch[2].pos * bary.z;
                o.pos = pos;
                o.uv = patch[0].uv * bary.x + patch[1].uv * bary.y + patch[2].uv * bary.z;
                o.worldPos = patch[0].worldPos * bary.x + patch[1].worldPos * bary.y + patch[2].worldPos * bary.z;
                return o;
            }

            // ---------------------
            // Hash function: returns a pseudo‐random value in [0,1] from a float2 input.
            float hash21(float2 p)
            {
                p = frac(p * float2(123.34, 456.21));
                p += dot(p, p + 45.32);
                return frac(p.x * p.y);
            }

            // ---------------------
            // Geometry Shader: Modify each tessellated triangle to simulate shredding.
            // ---------------------
            [maxvertexcount(3)]
            void geom(triangle v2f input[3], inout TriangleStream<v2f> triStream)
            {
                // For each vertex in the triangle...
                for (int i = 0; i < 3; i++)
                {
                    v2f o = input[i];
                    // Create a random factor based on the vertex UV and the EnemyID.
                    float randomFactor = hash21(o.uv + float2(_EnemyID, _EnemyID));
                    // Compute a downward offset that increases over time.
                    float offset = _ShredSpeed * _TimeScale * _Time.y * (0.5 + randomFactor);
                    o.worldPos.y -= offset;
                    // Recalculate clip-space position from the modified world position.
                    o.pos = UnityObjectToClipPos(o.worldPos);
                    triStream.Append(o);
                }
            }

            // ---------------------
            // Fragment Shader: Sample and output the texture color.
            // ---------------------
            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                return col;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
