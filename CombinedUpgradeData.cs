using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Characters.Player.Upgrades;
using UnityEngine;

namespace Characters.Player.Upgrades
{

    public class CombinedUpgradeData<TUpgradeTarget> : ScriptableObject
    {
        [SerializeField] private List<UpgradeBaseData<TUpgradeTarget>> _upgrades;
    }
}
