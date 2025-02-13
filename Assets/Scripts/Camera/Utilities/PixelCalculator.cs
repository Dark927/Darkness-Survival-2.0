using UnityEngine;

namespace Settings.CameraManagement
{
    public static class PixelCalculator
    {
        public static float RoundToNearestPixel(float unityUnits, Camera viewingCamera)
        {
            float valueInPixels = (Screen.height / (viewingCamera.orthographicSize * 2)) * unityUnits;
            valueInPixels = Mathf.Round(valueInPixels);
            float adjustedUnityUnits = valueInPixels / (Screen.height / (viewingCamera.orthographicSize * 2));
            return adjustedUnityUnits;
        }
    }
}
