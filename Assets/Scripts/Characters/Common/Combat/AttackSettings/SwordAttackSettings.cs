using Settings.CameraManagement.Shake;
using UnityEngine;

namespace Characters.Common.Combat
{
    [System.Serializable]
    public class SwordAttackSettings : AttackSettingsBase
    {
        [Space, Header("Heavy Attack Settings")]

        [SerializeField] private DamageSettings _heavyDamageSettings;
        [Space]
        [SerializeField] private ImpactSettings _heavyImpactSettings;

        public DamageSettings HeavyDamageSettings => _heavyDamageSettings;
        public ImpactSettings HeavyImpactSettings => _heavyImpactSettings;
    }
}
