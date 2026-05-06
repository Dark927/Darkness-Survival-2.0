
using System.Collections.Generic;
using Characters.Player.Upgrades;
using UnityEngine;

namespace UI.Characters.Upgrades
{
    public struct UpgradeInfoUI
    {
        public string Title;
        public string Type;
        public string Level;

        public List<StatUIInfo> StatsList;

        public string Description;
        public Sprite Icon;
    }
}
