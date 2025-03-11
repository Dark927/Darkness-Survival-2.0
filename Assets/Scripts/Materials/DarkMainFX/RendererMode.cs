using System;
using UnityEngine;


namespace Materials.DarkMainFX
{
    [Serializable]
    public enum RendererMode
    {
        IGNORE = -1,
        ORIGINAL = 0,
        STEP = 1,
        INTERP = 2,
        MASKSTEP = 3,
        MASKINTERP = 4,
        UNLIT = 5
    }
}
