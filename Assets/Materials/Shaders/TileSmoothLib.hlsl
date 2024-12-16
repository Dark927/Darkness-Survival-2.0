#ifndef TILE_SMOOTH_LIB_INCLUDED
#define TILE_SMOOTH_LIB_INCLUDED

#pragma require 2darray
/*
    Requirements:

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
    //pragmas
    #pragma shader_feature _USE_SMOOTHSTEP_ON
    #pragma shader_feature _USE_INTERPOLATION_ON
    #pragma shader_feature _USE_EDGE_DEBUG_ON
    #pragma shader_feature _USE_TILE_PADDING_ON
    
    //make sure it is texture array
    TEXTURE2D_ARRAY(_MainTex);
    SAMPLER(sampler_MainTex);
    
    //uniforms
    float _EdgeFactor;
    float4 _GridSize;
    StructuredBuffer<uint> _TileIndices;

    //include this
    #include "Assets/Materials/Shaders/TileSmoothLib.hlsl"
*/


float4 bilinear(float2 uv, float4 TL, float4 TR, float4 BL, float4 BR){
    float4 x1InterpColor = lerp(TL, TR, uv.x);
    float4 x2InterpColor = lerp(BL, BR, uv.x);
    float4 finalColor = lerp(x2InterpColor, x1InterpColor, uv.y);//directx -y
    return finalColor;
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


//just access indexed texture
float4 SampleTiled(Texture2DArray texArray, SamplerState texsampler, float2 uv)
{
    // Map vertex UV to the grid
    float2 gridUV = uv * _GridSize.xy;
    //calculate 9x9 tile
    float2 tileUV = fmod(uv * _GridSize.xy,1); // [0;1]

    // Determine the tile index (flattened array lookup)
    uint x = (uint)floor(gridUV.x);
    uint y = (uint)floor(gridUV.y);

    return SAMPLE_TEXTURE2D_ARRAY(texArray, texsampler, tileUV, indexate(x, y));
}

//returns a normal indexed via tile
float3 TileNormal(Texture2DArray normalMapArray, SamplerState normsampler, float2 uv)
{
    return UnpackNormal(SampleTiled(normalMapArray, normsampler, uv));
}

//returns unlit color of the tile
float4 TileSmooth(float2 uv)
{
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
        return color;
    #endif

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

    //smoothstep makes nicer transitions
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
        because of DirectX -Y coord
        bottom edges need colorT (although it is Top, but Bottom is sampled instead)
        same goes for top edges
        spent 2 nights over this crap
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

    return mixcolor;
}

#endif