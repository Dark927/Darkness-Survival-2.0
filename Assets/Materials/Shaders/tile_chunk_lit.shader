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
        _EdgeFactor("Edge Factor", Range(0.5, 1)) = 0.85
        
        [Header(Features)]
        [Toggle] _USE_INTERPOLATION ("Use interpolation", Float) = 1
        [Toggle] _USE_SMOOTHSTEP ("Use smoothstep", Float) = 1
        [Toggle] _USE_TILE_PADDING ("Use invisible tile padding", Float) = 1
        
        [Toggle] _USE_EDGE_DEBUG ("Debug tile bounds", Float) = 0
        
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
            #pragma shader_feature _USE_SMOOTHSTEP_ON
            #pragma shader_feature _USE_INTERPOLATION_ON
            #pragma shader_feature _USE_EDGE_DEBUG_ON
            #pragma shader_feature _USE_TILE_PADDING_ON
            
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
            float _EdgeFactor;

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



            float2 Rotate2DCoord(float2 coord, float2 center)
            {
                // Translate the coordinate system to the center
                float2 translated = coord - center;

                // Apply the 2D rotation matrix
                float2 rotated = float2(-translated.y, translated.x);

                // Translate the coordinate system back
                return rotated;// + center;
            }

            uint indexate(uint x, uint y)
            {
                x = min(x, (uint)_GridSize.x);
                y = min(y, (uint)_GridSize.y);
                
                #if _USE_TILE_PADDING_ON
                    return _TileIndices[(y+1) * ((uint)_GridSize.x + 2) + (x+1)];
                #else
                   return _TileIndices[y * (uint)_GridSize.x + x];
                #endif
            }


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

            #include "Packages/com.unity.render-pipelines.universal/Shaders/2D/Include/CombinedShapeLightShared.hlsl"

            float4 bilinear(float2 uv, float4 TL, float4 TR, float4 BL, float4 BR){
                float4 x1InterpColor = lerp(TL, TR, uv.x);
                float4 x2InterpColor = lerp(BL, BR, uv.x);
                float4 finalColor = lerp(x2InterpColor, x1InterpColor, uv.y);//directx -y
                return finalColor;
            }

            float4 frag(v2f i) : SV_Target
            {   
                float2 uv = i.uv;
                
                // Map vertex UV to the grid
                float2 gridUV = uv * _GridSize.xy;
                //calculate 9x9 tile
                float2 tileUV = fmod(uv * _GridSize.xy,1); // [0;1]

                // Determine the tile index (flattened array lookup)
                uint x = (uint)floor(gridUV.x);
                uint y = (uint)floor(gridUV.y);

                // Sample tiles
                float4 color =   SAMPLE_TEXTURE2D_ARRAY(_MainTex, sampler_MainTex, tileUV, indexate(x  , y));

                #if !_USE_INTERPOLATION_ON                
                    float4 mixcolor = color;
                #else

                    float4 colorL =  SAMPLE_TEXTURE2D_ARRAY(_MainTex, sampler_MainTex, tileUV, indexate(x-1, y));
                    float4 colorT =  SAMPLE_TEXTURE2D_ARRAY(_MainTex, sampler_MainTex, tileUV, indexate(x,   y-1));
                    float4 colorR =  SAMPLE_TEXTURE2D_ARRAY(_MainTex, sampler_MainTex, tileUV, indexate(x+1, y));
                    float4 colorB =  SAMPLE_TEXTURE2D_ARRAY(_MainTex, sampler_MainTex, tileUV, indexate(x,   y+1));
                    //corner tiles
                    float4 colorLB = SAMPLE_TEXTURE2D_ARRAY(_MainTex, sampler_MainTex, tileUV, indexate(x-1, y-1));
                    float4 colorRB = SAMPLE_TEXTURE2D_ARRAY(_MainTex, sampler_MainTex, tileUV, indexate(x+1, y-1));
                    float4 colorLT = SAMPLE_TEXTURE2D_ARRAY(_MainTex, sampler_MainTex, tileUV, indexate(x-1, y+1));
                    float4 colorRT = SAMPLE_TEXTURE2D_ARRAY(_MainTex, sampler_MainTex, tileUV, indexate(x+1, y+1));

                    float4 mixcolor = color;
                    float Iedging = 1 - _EdgeFactor;

                    float2 clipUV =  clamp( (      tileUV  - _EdgeFactor) / Iedging, 0.0, 1.0) * 0.5; //0.5 because we lerp both sides
                    float2 IclipUV = clamp( ( (1.0-tileUV) - _EdgeFactor) / Iedging, 0.0, 1.0) * 0.5;

                    //ця штука робить заебісь
                    #if _USE_SMOOTHSTEP_ON
                        clipUV = smoothstep(0.0, 1.0, clipUV);
                        IclipUV = smoothstep(0.0, 1.0, IclipUV);
                    #endif
                    
                    // #1 interpolate edges
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
                    
                    /*
                    через їблю з -Y координатами DirectX
                    у нижніх кутків треба colorT (хоч назва Top але семплюється ніфіга не топ)
                    ну і відповідно у верхніх colorB
                    */
                    float4 B_colorLB = bilinear(float2(IclipUV.x, 1 - IclipUV.y), color, colorL, colorT, colorLB);
                    float4 B_colorRB = bilinear(float2( clipUV.x, 1 - IclipUV.y), color, colorR, colorT, colorRB);
                    float4 B_colorLT = bilinear(float2(IclipUV.x, 1 -  clipUV.y), color, colorL, colorB, colorLT);
                    float4 B_colorRT = bilinear(float2( clipUV.x, 1 -  clipUV.y), color, colorR, colorB, colorRT);

                    mixcolor = lerp(mixcolor, B_colorLB, sign(diagonalWeights.x)); // Bottom-Left influence
                    mixcolor = lerp(mixcolor, B_colorRB, sign(diagonalWeights.y)); // Bottom-Right influence
                    mixcolor = lerp(mixcolor, B_colorLT, sign(diagonalWeights.z)); // Top-Left influence
                    mixcolor = lerp(mixcolor, B_colorRT, sign(diagonalWeights.w)); // Top-Right influence

                    
                    //draw rectangle outline based on tileUV
                    #if _USE_EDGE_DEBUG_ON
                        float bound = 0.996;
                        if(tileUV.x >= bound && tileUV.y <= bound){
                            mixcolor = float4(0,0,1,1); 
                        }
                        else if(tileUV.x <= bound && tileUV.y >= bound){
                            mixcolor = float4(0,1,0,1); 
                        }
                        else if(tileUV.x >= bound && tileUV.y >= bound){
                            mixcolor = float4(1,0,0,1); 
                        }
                    #endif
                #endif
                //lit return
                SurfaceData2D surfaceData;
                InputData2D inputData;

                InitializeSurfaceData(mixcolor.rgb, mixcolor.a, float4(0,0,0,0), surfaceData);
                InitializeInputData(uv, i.lightingUV, inputData);

                return CombinedShapeLightShared(surfaceData, inputData);
            
            }

            ENDHLSL
        }
    }
}