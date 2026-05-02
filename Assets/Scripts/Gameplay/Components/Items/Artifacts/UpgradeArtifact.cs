

using System.Collections.Generic;
using Characters.Common;
using Characters.Player.Upgrades;
using UnityEngine;

namespace Gameplay.Components.Items
{
    public class UpgradeArtifact : PickupItemBase
    {
        #region Fields 

        [SerializeField] private ItemUpgradeArtifactParameters _parameters = new();

        #endregion


        #region Properties

        public override IItemParameters Parameters => _parameters;

        #endregion


        #region Methods

        public override void Pickup(ICharacterLogic targetCharacter)
        {
            if (targetCharacter is IUpgradableCharacterLogic upgradableCharacter)
            {
                List<UpgradeProvider> upgradesProviders = new List<UpgradeProvider>();
                foreach (UpgradeConfigurationSO upgrade in _parameters.CustomUpgrades)
                {
                    upgradesProviders.Add(new UpgradeProvider(upgrade));
                }

                if (_parameters.AreIntroUpgrades)
                {
                    upgradableCharacter.ReceiveIntroUpgrades(upgradesProviders);
                }
                else
                {
                    upgradableCharacter.ReceiveCustomUpgrades(upgradesProviders);
                }
            }
        }

        #endregion
    }
}
