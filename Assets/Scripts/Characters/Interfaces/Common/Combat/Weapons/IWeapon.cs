using System;
using Characters.Common.Combat.Weapons.Data;
using Characters.Player.Upgrades;
using UnityEngine;

namespace Characters.Common.Combat.Weapons
{
    public interface IWeapon : IDisposable
    {
        GameObject GameObject { get; }
        public IAttackableEntityLogic Owner { get; }

        public void Initialize(WeaponAttackDataBase weaponAttackData);
        public void SetBasicDamageMultiplier(float multiplier);
    }
}
