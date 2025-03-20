
using UnityEngine;
using Utilities.Attributes;

namespace UI
{
    [System.Serializable]
    public struct ButtonAnimationParamsUI : IAnimationParamsUI
    {

        [SerializeField] private float _duration;

        [CustomHeader("Button Body", count: 1, depth: 1, headerColor: CustomHeaderAttribute.HeaderColor.cyan)]
        [SerializeField] private Vector2 _targetButtonScale;

        [Space, CustomHeader("Title", count: 1, depth: 1, headerColor: CustomHeaderAttribute.HeaderColor.cyan)]
        [SerializeField] private Color _targetTitleColor;

        [Space, CustomHeader("Borders", count: 1, depth: 1, headerColor: CustomHeaderAttribute.HeaderColor.cyan)]
        [SerializeField] private Color _targetBordersColor;


        public float DurationInSec { get => _duration; set => _duration = value; }
        public Vector2 TargetScale { get => _targetButtonScale; set => _targetButtonScale = value; }
        public Color TargetTitleColor { get => _targetTitleColor; set => _targetTitleColor = value; }
        public float TargetTitleSize { get; set; }
        public Color TargetBordersColor { get => _targetBordersColor; set => _targetBordersColor = value; }


        public ButtonAnimationParamsUI(float effectDuration = 1f)
        {
            _duration = effectDuration;
            _targetButtonScale = Vector2.one * 1.15f;
            _targetTitleColor = Color.white;
            _targetBordersColor = Color.white;
            TargetTitleSize = 1f;
        }

        public ButtonAnimationParamsUI(ButtonAnimationParamsUI parameters)
        {
            _duration = parameters.DurationInSec;
            _targetButtonScale = parameters.TargetScale;
            _targetTitleColor = parameters.TargetTitleColor;
            _targetBordersColor = parameters.TargetBordersColor;
            TargetTitleSize = parameters.TargetTitleSize;
        }
    }
}
