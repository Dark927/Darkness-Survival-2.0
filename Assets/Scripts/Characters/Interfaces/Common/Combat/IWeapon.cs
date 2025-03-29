using System;
using Characters.Common.Combat.Weapons.Data;
using Characters.Interfaces;
using Characters.Player.Upgrades;
using UnityEngine;

namespace Characters.Common.Combat.Weapons
{
    public interface IWeapon : IDisposable, IUpgradable
    {
        GameObject GameObject { get; }
        public IEntityDynamicLogic Owner { get; }

        public void Initialize(WeaponAttackDataBase weaponAttackData);
        public void SetCharacterDamageMultiplier(float damageMultiplier);
    }
}
