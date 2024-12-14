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

            //ця хрень працює але крутить текстури
            float4 SampleAtlasWithTiling(float2 tileUV, float2 tileOffset)
            {
                // Compute texel size and tile bounds/
                float2 texelSize = _MainTex_TexelSize.xy;
                //y стартує знизу
                //float2 minUV = float2(tileOffset.x,       1 - tileOffset.y - 0.5);
                //float2 maxUV = float2(tileOffset.x + 0.5, 1 - tileOffset.y); 

                //одна стоміліпіздрична флоата, 1 / 2^25
                //minUV.x += 2.9802326E-08;
                //maxUV.x += 2.9802326E-08;
                
                
                //float2 minUV = float2(0.0,0.5); //плитка (без полоски)
                //float2 maxUV = float2(0.5,1);

                float2 minUV = float2(0.5,0.5);//каменюка (полоска)
                float2 maxUV = float2(1,1);

                // Map tileUV to the full UV range of the tile
                float2 fullUV = lerp(minUV, maxUV, tileUV);

                // Calculate the fractional part of the UV and wrap it within tile bounds
                float2 wrappedUV = frac((fullUV - minUV) / (maxUV - minUV)) * (maxUV - minUV) + minUV;

                // Calculate texel coordinates
                float2 texelCoord = fullUV / texelSize;
                float2 texelCoordFloor = floor(texelCoord); // Bottom-left texel
                float2 texelCoordFract = frac(texelCoord); // Fractional part for blending

                //return float4(texelWrap((texelCoordFloor              )  * texelSize,minUV,float2(1, 1)).xy,0,1);
                float4 c00 = _MainTex.SampleLevel(sampler_MainTex, texelWrap((texelCoordFloor               ) * texelSize,minUV,maxUV), 0);
                float4 c10 = _MainTex.SampleLevel(sampler_MainTex, texelWrap((texelCoordFloor + float2(1, 0)) * texelSize,minUV,maxUV), 0);
                float4 c01 = _MainTex.SampleLevel(sampler_MainTex, texelWrap((texelCoordFloor + float2(0, 1)) * texelSize,minUV,maxUV), 0);
                float4 c11 = _MainTex.SampleLevel(sampler_MainTex, texelWrap((texelCoordFloor + float2(1, 1)) * texelSize,minUV,maxUV), 0);

                // Perform bilinear interpolation manually
                float4 colorX0 = lerp(c00, c10, texelCoordFract.x);
                float4 colorX1 = lerp(c01, c11, texelCoordFract.x);
                float4 finalColor = lerp(colorX0, colorX1, texelCoordFract.y);

                return c00;
            }



            float2 texturePointSmooth(float2 uvs)
            {
                //_MainTex_TexelSize Vector4(1 / width, 1 / height, width, height)
                uvs -= float2(_MainTex_TexelSize.x,_MainTex_TexelSize.y) * float2(0.5,0.5);
                float2 uv_pixels = uvs * float2(_MainTex_TexelSize.z,_MainTex_TexelSize.w);
                float2 delta_pixel = frac(uv_pixels) - float2(0.5,0.5);

                float2 ddxy = fwidth(uv_pixels);
                float2 mip = log2(ddxy) - 0.5;

                float2 clampedUV = uvs - (clamp(delta_pixel / ddxy, 0.0, 1.0) - delta_pixel) * float2(_MainTex_TexelSize.x,_MainTex_TexelSize.y);
                return clampedUV;
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
                float2 tileUV_fullrange = fmod(smoothUV * _GridSize.xy,1); // [0;1]
                float2 tileUV = tileuvgrid(tileUV_fullrange);  //[0;0.5] or [0.5;1]

                // Calculate tile atlas UVs

                float2 atlasUV = tileUV + atlasLookup(tileIndex);

                //return float4(atlasLookup(0).xy,0,1);
                return SampleAtlasWithTiling(tileUV_fullrange, float2(0.0,0.0)); 
                //return SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, atlasUV); 
                //rotate uv 90


                //atlasUV = Rotate2DCoord(atlasUV, float2(0.5,0.5));

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


                float edging = 0.8;
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