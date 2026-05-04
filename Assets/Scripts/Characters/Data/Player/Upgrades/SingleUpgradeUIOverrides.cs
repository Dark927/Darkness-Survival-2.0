using UnityEngine;

namespace Characters.Player.Upgrades
{
    [System.Serializable]
    public struct SingleUpgradeUIOverrides
    {
        [Header("Full Description Override (If checked, ignores the Stat Name and Value logic entirely and just displays the text below. " +
            "\n<color=yellow>Use color tags here for formatting</color>)")]
        public bool UseFullOverride;
        [TextArea(2, 4)]
        public string FullOverrideString;

        [Header("Upgrade Name Override (Overrides the default Upgrade name provided by the code)")]
        public string UpgradeNameOverride;

        [Header("Colors & Formatting")]
        public Color UpgradeNameColor;
        public Color UpgradeValueColor;
        public Color DowngradeValueColor;

        [Tooltip("If true, a '+' sign will use the Downgrade color (Red) and '-' will use the Upgrade color (Green). Ideal for Upgrades like Reload Time.")]
        public bool ReverseSignLogic;

        public static SingleUpgradeUIOverrides Default() => new SingleUpgradeUIOverrides
        {
            UpgradeNameColor = Color.white,
            UpgradeValueColor = Color.green,
            DowngradeValueColor = Color.red
        };
    }
}
