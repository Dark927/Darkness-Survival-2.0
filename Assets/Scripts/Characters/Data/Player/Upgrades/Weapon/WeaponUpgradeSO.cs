using Characters.Common.Combat.Weapons;
using System.Collections.Generic;
using UnityEngine;

namespace Characters.Player.Upgrades
{
    [CreateAssetMenu(fileName = "WeaponUpgradeData", menuName = "Game/Upgrades/Weapon Upgrades/Weapon Upgrade Data")]
    public class WeaponUpgradeSO : UpgradeSO
    {
        public IEnumerable<UpgradeLevelSO<IWeapon>> UpgradeLevels { get; }
    }
}
