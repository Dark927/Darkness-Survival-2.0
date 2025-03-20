
using UnityEngine;
using Utilities.Attributes;

namespace UI.Characters.Upgrades
{
    [System.Serializable]
    public struct CardAnimationParamsUI : IAnimationParamsUI
    {
        [SerializeField] private float _duration;

        [CustomHeader("Button Body", count: 1, depth: 1, headerColor: CustomHeaderAttribute.HeaderColor.cyan)]
        [SerializeField] private Vector2 _targetCardScale;

        [Space, CustomHeader("Title", count: 1, depth: 1, headerColor: CustomHeaderAttribute.HeaderColor.cyan)]
        [SerializeField] private Color _targetTitleColor;

        [Space, CustomHeader("Card", count: 2, depth: 1, headerColor: CustomHeaderAttribute.HeaderColor.cyan)]
        [SerializeField] private Color _targetCardColor;
        [SerializeField] private Color _targetCardIconColor;


        public float DurationInSec { get => _duration; set => _duration = value; }
        public Vector2 TargetScale { get => _targetCardScale; set => _targetCardScale = value; }
        public Color TargetTitleColor { get => _targetTitleColor; set => _targetTitleColor = value; }
        public Color TargetCardColor { get => _targetCardColor; set => _targetCardColor = value; }
        public Color TargetCardIconColor { get => _targetCardColor; set => _targetCardColor = value; }


        public CardAnimationParamsUI(float effectDuration = 1f)
        {
            _duration = effectDuration;
            _targetCardScale = Vector2.one * 1.15f;
            _targetTitleColor = Color.white;
            _targetCardColor = Color.white;
            _targetCardIconColor = Color.white;
        }
    }
}
