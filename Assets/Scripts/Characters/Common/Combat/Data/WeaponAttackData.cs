using UnityEngine;

namespace Characters.Common.Combat.Weapons.Data
{
    public abstract class WeaponAttackData : ScriptableObject
    {
        public abstract AttackSettingsBase AttackSettings { get; }
    }
}
