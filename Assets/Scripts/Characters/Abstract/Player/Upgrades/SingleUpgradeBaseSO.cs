

using Characters.Common.Combat.Weapons;
using Characters.Interfaces;
using UnityEngine;

namespace Characters.Player.Upgrades
{
    /// <summary>
    /// The base class for all upgrades where should be some stats, etc.
    /// </summary>
    public abstract class SingleUpgradeBaseSO<TUpgradeTarget> : ScriptableObject
    {
        // CharacterLogic, WeaponBase, ConcreteWeapon.
        public abstract void ApplyUpgrade(TUpgradeTarget target);
    }
}


