

using System;
using System.Collections.Generic;
using Characters.Player.Upgrades;

namespace UI.Characters.Upgrades
{
    public interface IUpgradeHandlerUI
    {
        public event EventHandler<UpgradeProvider> OnUpgradeSelected;

        public void DisplayUpgrades(IEnumerable<UpgradeProvider> upgradesData);
        public void HideUpgrades();
    }
}
