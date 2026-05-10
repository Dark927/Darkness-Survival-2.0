using System;
using System.Collections.Generic;
using UnityEngine;
using Utilities.Attributes;

namespace Characters.Player.Upgrades
{
    /// <summary>
    /// Serves as the generic blueprint for a distinct upgrade tier within a progression path.
    /// Utilizes contravariance to allow a mix of generic and highly specific upgrades within a single polymorphic inspector list.
    /// </summary>
    [Serializable]
    public abstract class UpgradeLevelData<TTarget> : IUpgradeLevelData where TTarget : IUpgradable
    {
        [CustomHeader("Main Settings", 0, 0)]
        [SerializeField, TextArea] private string _baseDescription;
        [SerializeField] private bool _useDowngradesInfoInDesc = true;

        [Space]
        [Tooltip("Polymorphic collection of upgrades. Accepts any AbstractUpgradeSO via the Unity Inspector.")]
        [SerializeField] private List<AbstractUpgradeSO> _upgrades;

        [Tooltip("Polymorphic collection of downgrades. Accepts any AbstractUpgradeSO via the Unity Inspector.")]
        [SerializeField] private List<AbstractUpgradeSO> _downgrades;

        [CustomHeader("Extra Settings (optional)", 0, 0)]
        [SerializeField] private Color _baseDescriptionColor = new Color(255, 230, 0, 255);
        [SerializeField] private UpgradeVisualDataUI _customVisualDataUI;
        [SerializeField] private Sprite _customIconUI;

        public List<StatUIInfo> UpgradeDetails => GetUpgradeDetails();
        public IEnumerable<AbstractUpgradeSO> Upgrades => _upgrades;
        public IEnumerable<AbstractUpgradeSO> Downgrades => _downgrades;
        public UpgradeVisualDataUI CustomUpgradeVisualDataUI => _customVisualDataUI;
        public Sprite CustomIconUI => _customIconUI;

        /// <summary>
        /// Iterates through the polymorphic upgrade collection and conditionally applies them 
        /// if the underlying SO implements the contravariant IUpgradeApplicator interface for the target type.
        /// </summary>
        /// <param name="target">The runtime entity receiving the upgrades.</param>
        public void ApplyAllUpgrades(TTarget target)
        {
            if (_upgrades == null) return;

            foreach (var upgrade in _upgrades)
            {
                if (upgrade is IUpgradeApplicator<TTarget> applicator)
                {
                    applicator.ApplyUpgrade(target);
                }
            }
        }

        /// <summary>
        /// Iterates through the polymorphic downgrade collection and conditionally applies them 
        /// if the underlying SO implements the contravariant IDowngradeApplicator interface for the target type.
        /// </summary>
        /// <param name="target">The runtime entity receiving the downgrades.</param>
        public void ApplyAllDowngrades(TTarget target)
        {
            if (_downgrades == null) return;

            foreach (var downgrade in _downgrades)
            {
                if (downgrade is IDowngradeApplicator<TTarget> applicator)
                {
                    applicator.ApplyDowngrade(target);
                }
            }
        }

        /// <summary>
        /// Aggregates formatting and statistical data from all contained upgrades and downgrades 
        /// to populate UI tooltips and upgrade selection panels.
        /// </summary>
        /// <returns>A structured list of UI statistics.</returns>
        public virtual List<StatUIInfo> GetUpgradeDetails()
        {
            var allStats = new List<StatUIInfo>();

            if (!string.IsNullOrEmpty(_baseDescription))
            {
                allStats.Add(new StatUIInfo
                {
                    StatName = _baseDescription,
                    NameColor = _baseDescriptionColor,
                    FormatTemplate = "{0}"
                });
            }

            if (_upgrades != null)
            {
                foreach (var upgrade in _upgrades)
                {
                    var upgradeInfoList = upgrade.GetUpgradeInfo();
                    if (upgradeInfoList != null && upgradeInfoList.Count > 0)
                    {
                        allStats.AddRange(upgradeInfoList);
                    }
                }
            }

            if (_useDowngradesInfoInDesc && _downgrades != null)
            {
                foreach (var downgrade in _downgrades)
                {
                    var downgradeInfoList = downgrade.GetDowngradeInfo();
                    if (downgradeInfoList != null && downgradeInfoList.Count > 0)
                    {
                        allStats.AddRange(downgradeInfoList);
                    }
                }
            }

            return allStats;
        }

        /// <summary>
        /// Non-generic entry point. Safely attempts to cast the generic target and apply upgrades.
        /// </summary>
        public void ApplyTo(IUpgradable target)
        {
            if (target is TTarget typedTarget)
            {
                ApplyAllUpgrades(typedTarget);
                ApplyAllDowngrades(typedTarget);
            }
            else
            {
                Debug.LogWarning($"[Architecture] Failed to apply upgrade. Target '{target.GetType().Name}' does not implement '{typeof(TTarget).Name}'.");
            }
        }


        /// <summary>
        /// Automatically runs in the Unity Editor to prevent designers from assigning 
        /// incompatible upgrades (e.g., Character upgrades into a Weapon list).
        /// </summary>
        public void ValidateEditorConstraints()
        {
            if (_upgrades != null)
            {
                for (int i = 0; i < _upgrades.Count; i++)
                {
                    var upgrade = _upgrades[i];

                    // If there's an asset, but it DOES NOT implement the applicator for this specific target
                    if (upgrade != null && !(upgrade is IUpgradeApplicator<TTarget>))
                    {
                        Debug.LogWarning($"[Architecture Guard] Rejected '{upgrade.name}'. It is not compatible with {typeof(TTarget).Name}.");
                        _upgrades[i] = null; // Instantly empties the slot in the Inspector
                    }
                }
            }

            if (_downgrades != null)
            {
                for (int i = 0; i < _downgrades.Count; i++)
                {
                    var downgrade = _downgrades[i];

                    if (downgrade != null && !(downgrade is IDowngradeApplicator<TTarget>))
                    {
                        Debug.LogWarning($"[Architecture Guard] Rejected downgrade '{downgrade.name}'. It is not compatible with {typeof(TTarget).Name}.");
                        _downgrades[i] = null; // Instantly empties the slot in the Inspector
                    }
                }
            }
        }
    }
}
