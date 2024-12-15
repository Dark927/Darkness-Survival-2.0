// This shader draws a texture on the mesh.
Shader "Custom/TileShaderLit"
{
    // The _BaseMap variable is visible in the Material's Inspector, as a field
    // called Base Map.
    Properties
    {
        _MainTex("Tile Atlas", 2DArray) = "white" {}
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

            // Compile the shader only on platforms that support texture arrays
            #pragma require 2darray

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

            TEXTURE2D_ARRAY(_MainTex);
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

            float2 Rotate2DCoord(float2 coord, float2 center)
            {
                // Translate the coordinate system to the center
                float2 translated = coord - center;

                // Apply the 2D rotation matrix
                float2 rotated = float2(-translated.y, translated.x);

                // Translate the coordinate system back
                return rotated;// + center;
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
                x = min(x, (uint)_GridSize.x);
                y = min(y, (uint)_GridSize.y);
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
                o.uv = v.uv;//TRANSFORM_TEX(v.uv, _MainTex);
                o.lightingUV = half2(ComputeScreenPos(o.pos / o.pos.w).xy);
                

                o.color = v.color * _Color * _RendererColor;
                return o;
            }

            float Area(float2 a, float2 b, float2 c) {
                return abs((a.x * (b.y - c.y) + b.x * (c.y - a.y) + c.x * (a.y - b.y)) * 0.5);
            }
            
            float4 GradientBarycentric(float2 uv) : SV_Target {
                //uv.x *= 0.5;
               // uv.y = uv.y * 0.5 + 0.5;
                // Define the corner colors
                float3 colorTopLeft = float3(1, 0, 0); // Red
                float3 colorTopRight = float3(0, 1, 0); // Green
                float3 colorBottomLeft = float3(0, 0, 1); // Blue
                
                // Calculate interpolated colors along the horizontal edges
                float3 topColor = lerp(colorTopLeft, colorTopRight, uv.x); // Interpolation at the top
                float3 bottomColor = lerp(colorBottomLeft, float3(0, 0, 0), uv.x); // Interpolation at the bottom
                
                // Interpolate vertically between the top and bottom colors
                float3 finalColor = lerp(bottomColor, topColor, uv.y);
                
                return float4(finalColor, 1.0); // Return as RGBA with full alpha
            }

            #include "Packages/com.unity.render-pipelines.universal/Shaders/2D/Include/CombinedShapeLightShared.hlsl"
            
            //хрень
            float2 GetUVWithPadding(float2 uv, float2 texelSize, float2 atlasRegionMin, float2 atlasRegionMax, float padding)
            {
                float2 paddedMin = atlasRegionMin - padding * texelSize;
                float2 paddedMax = atlasRegionMax - padding * texelSize;
                return clamp(lerp(paddedMin, paddedMax, uv),atlasRegionMin,atlasRegionMax);
            }

            float2 texelWrap(float2 coord, float2 minUV, float2 maxUV)
            {
                //minUV = tile(minUV); //ЗАЇБАЛИ КООРДИНАТИ DirectX
                //maxUV = minUV + float2(0.5,0.5);
                // Wrap coordinates to remain within the tile

                float2 ret =  frac((coord - minUV) / (maxUV - minUV)) * (maxUV - minUV) + minUV;
                //=ret.y = 1 - ret.y;
                //ret.x = 1 - ret.x;
                return ret;
            }

            float4 bilinear(float2 uv, float4 TL, float4 TR, float4 BL, float4 BR){
                float4 x1InterpColor = lerp(TL, TR, uv.x);
                float4 x2InterpColor = lerp(BL, BR, uv.x);
                float4 finalColor = lerp(x2InterpColor, x1InterpColor, uv.y);//directx -y
                return finalColor;
            }

            float4 bilinearGL(float2 uv, float4 TL, float4 TR, float4 BL, float4 BR){
                return float4(uv.xy,0,1);
            }
            float4 bilinearDX(float2 uv, float4 TL, float4 TR, float4 BL, float4 BR){
                return float4(uv.x, 1 - uv.y,0,1);
            }


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
                float2 tileUV = fmod(smoothUV * _GridSize.xy,1); // [0;1]


                // Sample tiles
                float4 color =  SAMPLE_TEXTURE2D_ARRAY(_MainTex, sampler_MainTex, tileUV, tileIndex);
                float4 colorL = SAMPLE_TEXTURE2D_ARRAY(_MainTex, sampler_MainTex, tileUV, indexate(x-1, y));
                float4 colorT = SAMPLE_TEXTURE2D_ARRAY(_MainTex, sampler_MainTex, tileUV, indexate(x,   y-1));
                float4 colorR = SAMPLE_TEXTURE2D_ARRAY(_MainTex, sampler_MainTex, tileUV, indexate(x+1, y));
                float4 colorB = SAMPLE_TEXTURE2D_ARRAY(_MainTex, sampler_MainTex, tileUV, indexate(x,   y+1));
                //corner tiles
                float4 colorLB = SAMPLE_TEXTURE2D_ARRAY(_MainTex, sampler_MainTex, tileUV, indexate(x-1, y-1));
                float4 colorRB = SAMPLE_TEXTURE2D_ARRAY(_MainTex, sampler_MainTex, tileUV, indexate(x+1, y-1));
                float4 colorLT = SAMPLE_TEXTURE2D_ARRAY(_MainTex, sampler_MainTex, tileUV, indexate(x-1, y+1));
                float4 colorRT = SAMPLE_TEXTURE2D_ARRAY(_MainTex, sampler_MainTex, tileUV, indexate(x+1, y+1));

                float4 mixcolor = color;
                float edging = 0.85;
                float Iedging = 1 - edging;

                float2 clipUV =  clamp( (      tileUV  - edging) / Iedging, 0.0, 1.0) * 0.5; //0.5 because we lerp both sides
                float2 IclipUV = clamp( ( (1.0-tileUV) - edging) / Iedging, 0.0, 1.0) * 0.5;

                clipUV = smoothstep(0.0, 1.0, clipUV);
                IclipUV = smoothstep(0.0, 1.0, IclipUV);

                mixcolor = lerp(mixcolor, colorL, IclipUV.x); 
                mixcolor = lerp(mixcolor, colorT, IclipUV.y); 
                mixcolor = lerp(mixcolor, colorR, clipUV.x); 
                mixcolor = lerp(mixcolor, colorB, clipUV.y); 


                // Diagonal weights based on overlap of clipUV and IclipUV
                float4 diagonalWeights = float4(
                    IclipUV.x * IclipUV.y, // Left-Bottom weight
                    clipUV.x * IclipUV.y,  // Right-Bottom weight
                    IclipUV.x * clipUV.y,  // Left-Top weight
                    clipUV.x * clipUV.y    // Right-Top weight
                );
                
                // Add corner contributions
                float coef = 153377335;
                //mind the swapped colors!
                float4 B_colorLB = bilinear(float2(IclipUV.x, 1 - IclipUV.y), color, colorL, colorT, colorLB);
                float4 B_colorRB = bilinear(float2( clipUV.x, 1 - IclipUV.y), color, colorR, colorT, colorRB); // right bottom angle

                float4 B_colorLT = bilinear(float2(IclipUV.x, 1 -  clipUV.y), color, colorL, colorB, colorLT);
                float4 B_colorRT = bilinear(float2( clipUV.x, 1 -  clipUV.y), color, colorR, colorB, colorRT);

                mixcolor = lerp(mixcolor, B_colorLB, saturate(diagonalWeights.x * coef)); // Bottom-Left influence
                mixcolor = lerp(mixcolor, B_colorRB, saturate(diagonalWeights.y * coef)); // Bottom-Right influence
                mixcolor = lerp(mixcolor, B_colorLT, saturate(diagonalWeights.z * coef)); // Top-Left influence
                mixcolor = lerp(mixcolor, B_colorRT, saturate(diagonalWeights.w * coef)); // Top-Right influence

                //mixcolor = lerp(color,mixcolor, 0.7); 

                
                //draw rectangle outline based on tileUV
                float bound = 0.996;
                if(1==1){

                }
                else if(tileUV.x > bound && tileUV.y < bound){
                    mixcolor = float4(0,0,1,1); 
                }
                else if(tileUV.x < bound && tileUV.y > bound){
                    mixcolor = float4(0,1,0,1); 
                }
                
                

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