// This shader draws a texture on the mesh.
Shader "Custom/TileShaderLit"
{
    // The _BaseMap variable is visible in the Material's Inspector, as a field
    // called Base Map.
    Properties
    {
        _MainTex ("Tile Atlas", 2D) = "white" {}
        _NormalMap("Normal Map", 2D) = "bump" {}
        // x: Grid width, y: Grid height, z: Atlas width, w: Atlas height
        _GridSize("Grid Size", Vector) = (9,9,2,2)

        [HideInInspector] _Color("Tint", Color) = (1,1,1,1)
        [HideInInspector] _RendererColor("RendererColor", Color) = (1,1,1,1)
    }

    SubShader
    {   
        //for lit
        //Tags {"Queue" = "Transparent" "RenderType" = "Transparent" "RenderPipeline" = "UniversalPipeline" }
        Tags { "RenderType" = "Opaque" "RenderPipeline" = "UniversalPipeline" "Queue" = "Geometry"}

        //for lit
        Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
        Cull Off
        ZWrite Off
        Pass
        {
            Tags { "LightMode" = "Universal2D" }

            HLSLPROGRAM
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            #pragma vertex vert
            #pragma fragment frag

            #pragma multi_compile USE_SHAPE_LIGHT_TYPE_0 __
            #pragma multi_compile USE_SHAPE_LIGHT_TYPE_1 __
            #pragma multi_compile USE_SHAPE_LIGHT_TYPE_2 __
            #pragma multi_compile USE_SHAPE_LIGHT_TYPE_3 __
            #pragma multi_compile _ DEBUG_DISPLAY
            
            //#include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float4 color  : COLOR;
                float2 uv : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float4 color: COLOR;
                float2 uv : TEXCOORD0;
                float2 lightingUV : TEXCOORD1;

                #if defined(DEBUG_DISPLAY)
                float3  positionWS  : TEXCOORD2;
                #endif
                UNITY_VERTEX_OUTPUT_STEREO
            };


            #include "Packages/com.unity.render-pipelines.universal/Shaders/2D/Include/LightingUtility.hlsl"

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);
            TEXTURE2D(_NormalMap);
            SAMPLER(sampler_NormalMap);
            float4 _NormalMap_ST;

            float4 _MainTex_ST;
            float4 _MainTex_TexelSize;

            float4 _Color;
            half4 _RendererColor;

            #if USE_SHAPE_LIGHT_TYPE_0
            SHAPE_LIGHT(0)
            #endif

            #if USE_SHAPE_LIGHT_TYPE_1
            SHAPE_LIGHT(1)
            #endif

            #if USE_SHAPE_LIGHT_TYPE_2
            SHAPE_LIGHT(2)
            #endif

            #if USE_SHAPE_LIGHT_TYPE_3
            SHAPE_LIGHT(3)
            #endif


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
                v2f o = (v2f)0;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

                o.pos = TransformObjectToHClip(v.vertex);
                #if defined(DEBUG_DISPLAY)
                o.positionWS = TransformObjectToWorld(v.positionOS);
                #endif
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.lightingUV = half2(ComputeScreenPos(o.pos / o.pos.w).xy);
                

                o.color = v.color * _Color * _RendererColor;
                return o;
            }

            float4 goodlerp(float4 a, float4 b, float t)
            {
                return lerp(a,b,smoothstep(0.0,1.0,t));
            }

            #include "Packages/com.unity.render-pipelines.universal/Shaders/2D/Include/CombinedShapeLightShared.hlsl"
            
            float4 frag(v2f i) : SV_Target
            {   
                float2 smoothUV = i.uv;
                
                // Map vertex UV to the grid
                float2 gridUV = smoothUV * _GridSize.xy;
                  
                // Determine the tile index (flattened array lookup)
                uint x = (uint)floor(gridUV.x);
                uint y = (uint)floor(gridUV.y);
                uint tileIndex = indexate(x, y);

                //calculate 9x9 tile
                float2 tileUV_fullrange = fmod(smoothUV * _GridSize.xy,1); 
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


                float4 color =  SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, atlasUV); 
                float4 colorL = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, atlasUV_L);
                float4 colorT = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, atlasUV_T);
                float4 colorR = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, atlasUV_R);
                float4 colorB = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, atlasUV_B);

                float4 colorRB = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, atlasUV_LT); //RB
                float4 colorLB = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, atlasUV_RT); //LB
                float4 colorRT = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, atlasUV_LB); //RT
                float4 colorLT = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, atlasUV_RB); //LT

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
                /*
                if(1==1){

                }
                else if(tileUV_fullrange.x > 0.996f && tileUV_fullrange.y < 0.996f){
                    mixcolor = float4(0,0,1,1); 
                }
                else if(tileUV_fullrange.x < 0.996f && tileUV_fullrange.y > 0.996f){
                    mixcolor = float4(0,1,0,1); 
                }
                */
                

                //lit return
                SurfaceData2D surfaceData;
                InputData2D inputData;

                InitializeSurfaceData(mixcolor.rgb, mixcolor.a, float4(0,0,0,0), surfaceData);
                InitializeInputData(smoothUV, i.lightingUV, inputData);

                return CombinedShapeLightShared(surfaceData, inputData);
            
            }

            ENDHLSL
        }
    }
}