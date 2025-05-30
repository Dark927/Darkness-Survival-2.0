﻿

using UnityEngine.Rendering.Universal;

namespace Characters.Interfaces
{
    public interface IEntityLight
    {
        public float LightIntensityLimit { get; }
        public float CurrentLightIntensity { get; }

        public void SetLightIntensity(float lightIntensity);
        public void SetLightIntensityLimit(float lightIntensityLimit);
        public void UpdateLightRadius(float multiplier);
    }
}
