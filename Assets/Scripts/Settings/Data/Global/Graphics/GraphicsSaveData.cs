using System;
using UnityEngine;

[Serializable]
public class GraphicsSaveData
{
    // sensible defaults so if no save file exists, the game still works
    public int VSyncCount = 1;
    public int TargetFrameRate = 60;
    public FullScreenMode DisplayMode = FullScreenMode.ExclusiveFullScreen;

    public int ResolutionWidth = 1920;
    public int ResolutionHeight = 1080;
}
