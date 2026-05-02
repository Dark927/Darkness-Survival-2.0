using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Characters.Common
{
    [RequireComponent(typeof(Light2D))]
    public class EntityLight : IEntityLight
    {
        private Light2D _light;
        private float _lightIntensityLimit;
        private float _targetLightIntensity;

        public float LightIntensityLimit => _lightIntensityLimit;
        public float CurrentLightIntensity => _light.intensity;

        public EntityLight(Light2D light)
        {
            _light = light;
        }

        public void SetLightIntensity(float targetLightIntensity)
        {
            if (targetLightIntensity < 0f)
            {
                return;
            }

            SetCurrentLightIntensity(targetLightIntensity);
        }

        public void SetLightIntensityLimit(float targetLightIntensityLimit)
        {
            if (targetLightIntensityLimit < 0f)
            {
                return;
            }

            _targetLightIntensity += targetLightIntensityLimit - _lightIntensityLimit;
            _lightIntensityLimit = targetLightIntensityLimit;
            SetCurrentLightIntensity(_targetLightIntensity);
        }

        private void SetCurrentLightIntensity(float targetLightIntensity)
        {
            _targetLightIntensity = targetLightIntensity;

            if (_targetLightIntensity > _lightIntensityLimit)
            {
                _targetLightIntensity = _lightIntensityLimit;
            }

            _light.intensity = _targetLightIntensity;

        }

        public void UpdateLightRadius(float multiplier)
        {
            _light.pointLightInnerRadius *= multiplier;
            _light.pointLightOuterRadius *= multiplier;
        }
    }
}
