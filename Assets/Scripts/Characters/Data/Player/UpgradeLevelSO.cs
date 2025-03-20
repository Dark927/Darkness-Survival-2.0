using System.Collections.Generic;
using UnityEngine;

namespace Characters.Player.Upgrades
{
    public abstract class UpgradeLevelSO<TTarget> : ScriptableObject
    {
        [SerializeField] private List<SingleUpgradeBaseSO<TTarget>> _upgrades;

        [SerializeField] private List<SingleUniversalUpgradeSO<TTarget>> _downgrades;

        public IEnumerable<SingleUpgradeBaseSO<TTarget>> Upgrades => _upgrades;
        public IEnumerable<SingleUniversalUpgradeSO<TTarget>> Downgrades => _downgrades;
    }
}
