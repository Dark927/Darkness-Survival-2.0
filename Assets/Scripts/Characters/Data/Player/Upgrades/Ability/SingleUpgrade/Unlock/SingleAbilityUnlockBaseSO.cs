
using UnityEngine;

namespace Characters.Player.Upgrades
{
    public abstract class SingleAbilityUnlockBaseSO : SingleUpgradeBaseSO<IUpgradableCharacterLogic>
    {
        [SerializeField] private UpgradeConfigurationSO _abilityUpgradeConfiguration;

        public virtual int AbilityID => -1;
        public UpgradeConfigurationSO AbilityUpgradeConfiguration => _abilityUpgradeConfiguration;
    }
}
