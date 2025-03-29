
using Characters.Common.Combat.Weapons;
using Materials;
using UnityEngine;

namespace Characters.Player.Upgrades
{
    [CreateAssetMenu(fileName = "NewNeroWeaponAuraUpgradeSO", menuName = "Game/Upgrades/Weapon Upgrades/Single Upgrades/Nero/Weapon Aura Upgrade Data")]
    public class NeroWeaponAuraUpgradeSO : SingleUpgradeBaseSO<IUpgradableWeapon>
    {
        [SerializeField] private string _auraDescription;
        [SerializeField] private ScriptableMaterialPropsBase _auraMaterialPropsData;

        protected override string GetInfo(char sign)
        {
            return _auraDescription;
        }

        public override void ApplyUpgrade(IUpgradableWeapon target)
        {
            if (_auraMaterialPropsData == null)
            {
                return;
            }

            var concreteWeapon = target as IConcreteEntityWeapon<NeroLogic>;

            if (concreteWeapon != null)
            {
                concreteWeapon.ApplySpecialAura(_auraMaterialPropsData);
            }
        }
    }
}
