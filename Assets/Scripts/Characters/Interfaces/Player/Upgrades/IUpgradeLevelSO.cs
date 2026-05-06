

using System.Collections.Generic;
using UnityEngine;

namespace Characters.Player.Upgrades
{
    /// <summary>
    /// This empty interface used in the abstract UpgradeSO.
    /// We need to cast it to the concrete upgrade level (if we want to access all upgrades inside it).
    /// </summary>
    public interface IUpgradeLevelSO
    {
        public List<StatUIInfo> UpgradeDetails { get; }
        public UpgradeVisualDataUI CustomUpgradeVisualDataUI { get; }

        // Custom Icon separated from UpgradeVisualDataUI for better reuse
        public Sprite CustomIconUI { get; }
    }
}
