

using Characters.Player.Upgrades;
using System;
using System.Collections.Generic;

namespace UI.Characters.Upgrades
{
    public interface IUpgradeHandlerUI
    {
        public event EventHandler<UpgradeProvider> OnUpgradeSelected;

        public void DisplayUpgrades(IEnumerable<UpgradeProvider> upgradesData);
        public void HideUpgrades();
    }
}
