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
        DISSOLVE = 4,
        HSVREPLACE = 8,
        HSVINTERP = 16,
        HSVMASK = 32,
        JITTERFREE = 64,
        TINT = 128,



        FADE = 128,
        OUTLINE = 128,
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
