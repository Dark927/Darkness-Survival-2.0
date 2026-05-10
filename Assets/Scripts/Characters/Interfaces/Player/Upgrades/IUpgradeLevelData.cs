using System.Collections.Generic;
using UnityEngine;

namespace Characters.Player.Upgrades
{
    public interface IUpgradeLevelData
    {
        List<StatUIInfo> UpgradeDetails { get; }
        UpgradeVisualDataUI CustomUpgradeVisualDataUI { get; }
        Sprite CustomIconUI { get; }

        void ValidateEditorConstraints();

        IEnumerable<AbstractUpgradeSO> Upgrades { get; }
        IEnumerable<AbstractUpgradeSO> Downgrades { get; }

        void ApplyTo(IUpgradable target);
    }
}
