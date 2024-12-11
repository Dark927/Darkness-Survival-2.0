// This shader draws a texture on the mesh.
Shader "Custom/TileShader"
{
    // The _BaseMap variable is visible in the Material's Inspector, as a field
    // called Base Map.
    Properties
    {
        _MainTex ("Tile Atlas", 2D) = "white" {}
        // x: Grid width, y: Grid height, z: Atlas width, w: Atlas height
        _GridSize("Grid Size", Vector) = (9,9,2,2)
    }

    SubShader
    {
        Tags { "RenderType" = "Opaque" "RenderPipeline" = "UniversalPipeline" "Queue" = "Geometry"}

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            //#include "UnityCG.cginc"

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

            sampler2D _MainTex;
            float4 _MainTex_ST;

            float4 _GridSize; // x: Grid width, y: Grid height, z: Atlas width, w: Atlas height

            
            StructuredBuffer<uint> _TileIndices;

            //CBUFFER_START(UnityPerMaterial)
            //    uint _TileIndices[81]; // 9x9 tile indices, for simplicity using 81 elements
            //CBUFFER_END

            //pizda
            float2 tile(float x, float y){
                return float2(x, -y);
            }
            float2 tile(float2 coord){
                return float2(coord.x, -coord.y);
            }
            //endpizda

            float2 tileuvgrid(float2 tileUV){
                tileUV /= _GridSize.zw;
                tileUV.y = 1 - tileUV.y;
                return tileUV;
            }

            float2 atlasLookup(uint tileIndex){
                float2 atlasUV = float2(
                    tileIndex % (uint)_GridSize.z,
                    tileIndex / (uint)_GridSize.z
                );

                atlasUV /= _GridSize.zw; // Normalize to atlas coordinates
                return tile(atlasUV);
            }


            uint indexate(uint x, uint y)
            {
                return _TileIndices[y * (uint)_GridSize.x + x];
            }

            ///


            v2f vert(appdata v)
            {
                v2f o;
                o.pos = TransformObjectToHClip(v.vertex);
                o.uv = v.uv;
                return o;
            }

            float4 goodlerp(float4 a, float4 b, float t)
            {
                return lerp(a,b,smoothstep(0.0,1.0,t));
            }

            float4 frag(v2f i) : SV_Target
            {
                // Map vertex UV to the grid
                float2 gridUV = i.uv * _GridSize.xy;

                // Determine the tile index (flattened array lookup)
                uint x = (uint)floor(gridUV.x);
                uint y = (uint)floor(gridUV.y);
                uint tileIndex = indexate(x, y);

                //calculate 9x9 tile
                float2 tileUV_fullrange = fmod(i.uv * _GridSize.xy,1); 
                float2 tileUV = tileuvgrid(tileUV_fullrange);

                // Calculate tile atlas UVs

                float2 atlasUV =   tileUV + atlasLookup(tileIndex);

                float2 atlasUV_L = tileUV + atlasLookup(indexate(x-1, y));
                float2 atlasUV_T = tileUV + atlasLookup(indexate(x,   y-1));
                float2 atlasUV_R = tileUV + atlasLookup(indexate(x+1, y));
                float2 atlasUV_B = tileUV + atlasLookup(indexate(x,   y+1));

                float2 atlasUV_LT = tileUV + atlasLookup(indexate(x-1, y-1));
                float2 atlasUV_RT = tileUV + atlasLookup(indexate(x+1, y-1));
                float2 atlasUV_LB = tileUV + atlasLookup(indexate(x-1, y+1));
                float2 atlasUV_RB = tileUV + atlasLookup(indexate(x+1, y+1));


                float4 color = tex2D(_MainTex, atlasUV); 
                float4 colorL = tex2D(_MainTex, atlasUV_L);
                float4 colorT = tex2D(_MainTex, atlasUV_T);
                float4 colorR = tex2D(_MainTex, atlasUV_R);
                float4 colorB = tex2D(_MainTex, atlasUV_B);

                float4 colorRB = tex2D(_MainTex, atlasUV_LT); //RB
                float4 colorLB = tex2D(_MainTex, atlasUV_RT); //LB
                float4 colorRT = tex2D(_MainTex, atlasUV_LB); //RT
                float4 colorLT = tex2D(_MainTex, atlasUV_RB); //LT

                float4 mixcolor = color;
                float2 one_Minus_tileUV = 1 - tileUV_fullrange;

                float2 sm = smoothstep(tileUV_fullrange.x,0.9,1.0);
                float2 smInv = smoothstep(1-tileUV_fullrange.x,0.9,1.0);


                float edging = 0.9;
                float Iedging = 1 - edging;
                
                float2 clipUV =  clamp( (      tileUV_fullrange  - edging) / Iedging, 0.0, 1.0) * 0.5; //0.5 because we lerp both sides
                float2 IclipUV = clamp( ( (1.0-tileUV_fullrange) - edging) / Iedging, 0.0, 1.0) * 0.5;

                
                mixcolor = lerp(mixcolor, colorL, IclipUV.x); 
                mixcolor = lerp(mixcolor, colorT, IclipUV.y); 
                mixcolor = lerp(mixcolor, colorR, clipUV.x); 
                mixcolor = lerp(mixcolor, colorB, clipUV.y); 
                
                
                // Separate corner weights to resolve overlaps
                float cornerWeightTL = min(IclipUV.x, IclipUV.y); // Top-Left corner
                float cornerWeightTR = min(clipUV.x, IclipUV.y);  // Top-Right corner
                float cornerWeightBL = min(IclipUV.x, clipUV.y);  // Bottom-Left corner
                float cornerWeightBR = min(clipUV.x, clipUV.y);  // Bottom-Right corner

                // Add corner contributions
                //mixcolor = lerp(mixcolor, color, cornerWeightTL); // Top-Left influence
                //mixcolor = lerp(mixcolor, color, cornerWeightTR); // Top-Right influence
                //mixcolor = lerp(mixcolor, color, cornerWeightBL); // Bottom-Left influence
                //mixcolor = lerp(mixcolor, color, cornerWeightBR); // Bottom-Right influence

                //mixcolor = lerp(color,mixcolor, 0.8); 

                
                //draw rectangle outline based on tileUV_fullrange
                if(1==1){

                }
                else if(tileUV_fullrange.x > 0.996f && tileUV_fullrange.y < 0.996f){
                    mixcolor = float4(0,0,1,1); 
                }
                else if(tileUV_fullrange.x < 0.996f && tileUV_fullrange.y > 0.996f){
                    mixcolor = float4(0,1,0,1); 
                }
                //*/
                





                //return float4(clipUV.xy,0,1);
                return mixcolor;

            }

            ENDHLSL
        }
    }
}