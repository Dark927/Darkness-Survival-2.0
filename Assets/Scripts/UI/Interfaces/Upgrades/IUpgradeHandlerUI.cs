

using Characters.Player.Upgrades;
using System;
using System.Collections.Generic;

namespace UI.Characters.Upgrades
{
    public interface IUpgradeHandlerUI
    {
        public event EventHandler<UpgradeConfigurationSO> OnUpgradeSelected;

        public void DisplayUpgrades(IEnumerable<UpgradeConfigurationSO> upgradesData);
        public void HideUpgrades();
    }
}
