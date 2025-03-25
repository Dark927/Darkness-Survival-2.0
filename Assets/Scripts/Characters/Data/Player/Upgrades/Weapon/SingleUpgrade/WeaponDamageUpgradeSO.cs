
using Characters.Common.Combat.Weapons;
using UnityEngine;

namespace Characters.Player.Upgrades
{
    [CreateAssetMenu(fileName = "NewWeaponDamageUpgradeSO", menuName = "Game/Upgrades/Weapon Upgrades/Single Upgrades/Damage Upgrade Data")]
    public class WeaponDamageUpgradeSO : SingleUniversalUpgradeSO<IWeapon>
    {
        [SerializeField, Min(0)] private float _damageUpgradePercent = 0;

        protected override string GetInfo(char sign)
        {
            return $"Max DMG : {sign}{_damageUpgradePercent}%";
        }

        public override void ApplyUpgrade(IWeapon target)
        {
            Apply(target, (1 + (_damageUpgradePercent / 100f)));
        }

        public override void ApplyDowngrade(IWeapon target)
        {
            Apply(target, (1 - (_damageUpgradePercent / 100f)));
        }

        private void Apply(IWeapon target, float damageMult)
        {
            target.ApplyNewDamagePercent(damageMult);
        }
    }
}
