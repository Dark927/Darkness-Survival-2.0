using System;
using UnityEngine;
using Utilities.Attributes;

namespace Characters.Common.Combat
{
    [Serializable]
    public class ReactiveHazardAttackSettings : HazardAttackSettings
    {
        [Space, CustomHeader("Reactive Settings", "Settings for marking enemies and death reactions.", 2, 1, CustomHeaderAttribute.HeaderColor.red)]

        [Tooltip("The floating icon prefab that appears over marked enemies.")]
        [SerializeField] private GameObject _markVisualPrefab;

        [Tooltip("How long the mark (and visual) stays on the enemy after being applied. Can overlap into Reload time!")]
        [SerializeField, Min(0.1f)] private float _effectDurationTimeSec = 10f;

        public GameObject MarkVisualPrefab { get => _markVisualPrefab; set => _markVisualPrefab = value; }
        public float EffectDurationTimeSec { get => _effectDurationTimeSec; set => _effectDurationTimeSec = value; }

        public ReactiveHazardAttackSettings(ReactiveHazardAttackSettings source) : base(source)
        {
            _markVisualPrefab = source.MarkVisualPrefab;
            _effectDurationTimeSec = source.EffectDurationTimeSec;
        }

        public override IAttackSettings Clone()
        {
            return new ReactiveHazardAttackSettings(this);
        }
    }
}
