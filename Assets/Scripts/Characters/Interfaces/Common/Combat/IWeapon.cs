using System;
using Characters.Common.Combat.Weapons.Data;
using UnityEngine;

namespace Characters.Common.Combat.Weapons
{
    // ToDo : extend this for upgrades applying.
    public interface IWeapon : IDisposable
    {
        GameObject GameObject { get; }
        public void Initialize(WeaponAttackDataBase weaponAttackData);
        public void SetCharacterDamageMultiplier(float damageMultiplier);
        public void ApplyNewDamagePercent(float damagePercent);
    }
}
