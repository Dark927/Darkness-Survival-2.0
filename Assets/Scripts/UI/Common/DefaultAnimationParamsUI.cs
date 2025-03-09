

using UnityEngine;
using Utilities.Attributes;

namespace UI.CustomCursor
{
    [System.Serializable]
    public struct DefaultAnimationParamsUI : IAnimationParamsUI
    {
        [SerializeField] private float _durationInSec;

        [CustomHeader("Body", count: 2, depth: 2, headerColor: CustomHeaderAttribute.HeaderColor.cyan)]
        [SerializeField] private Vector2 _targetScale;
        [SerializeField] private Color _targetColor;

        public float DurationInSec { get => _durationInSec; set => _durationInSec = value; }
        public Vector2 TargetScale { get => _targetScale; set => _targetScale = value; }
        public Color TargetColor { get => _targetColor; set => _targetColor = value; }


        public DefaultAnimationParamsUI(float duration = 1f)
        {
            _durationInSec = duration;
            _targetScale = Vector2.one;
            _targetColor = Color.white;
        }
    }
}
