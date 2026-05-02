using System;
using UnityEngine;


namespace Materials.DarkMainFX
{
    [Serializable]
    public enum RendererMode
    {
        IGNORE = 0,
        ORIGINAL = 1,
        NOFX = 2,
        UNLIT = 3
    }

    [Serializable]
    [Flags]
    public enum ShaderFeatures
    {
        NONE = 0,
        FLASH = 1,
        EMISSION = 2,
        EMISSION_TEX = 4,
        DISSOLVE = 8,
        HSVREPLACE = 16,
        HSVINTERP = 32,
        HSVMASK = 64,
        JITTERFREE = 128,
        TINT = 256,
        OUTLINE = 512,


        //TODO:
        FADE = 128,
        GRADIENT = 128,
        PIXELATE = 256,
        BLUR = 512,
        HOLOGRAM = 1024,
        ABBERATION = 2048,
        GLITCH = 4096,
        SHADOW = 8192,
        WAVE = 16384,
        VERTEXRESIZE = 32768,
        DISTORTION = 65536,







    }
}
