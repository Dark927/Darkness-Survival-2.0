
using System.Collections.Generic;
using UnityEngine;

namespace Characters.Player.Upgrades
{
    /// <summary>
    /// Base clase for Upgrades SO, used as container in the Inspector (to receive different upgrades)
    /// </summary>

    public abstract class UpgradeSO : ScriptableObject
    {
        public abstract IEnumerable<IUpgradeLevelSO> UpgradeLevels { get; }
    }
}
