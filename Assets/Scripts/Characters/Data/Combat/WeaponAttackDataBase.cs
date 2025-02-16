using UnityEngine;

namespace Characters.Common.Combat.Weapons.Data
{
    public abstract class WeaponAttackDataBase : ScriptableObject
    {
        public abstract AttackSettingsBase AttackSettings { get; }
    }
}
