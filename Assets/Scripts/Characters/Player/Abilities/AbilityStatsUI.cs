using UnityEngine;

namespace Characters.Common.Abilities
{
    [System.Serializable]
    public struct AbilityStatsUI
    {
        #region Fields

        // These are the raw fields the designer sees in the Unity Inspector.
        // We keep them private so no outside script can read an empty string by mistake.

        [Tooltip("Leave blank to use default 'Effect Power'")]
        [SerializeField] private string _strengthUIName;

        [Tooltip("Leave blank to use default 'Radius'")]
        [SerializeField] private string _radiusUIName;

        [Tooltip("Leave blank to use default 'Duration'")]
        [SerializeField] private string _durationUIName;

        #endregion

        #region Properties (The Fallback Logic)

        public string StrengthUIName => string.IsNullOrWhiteSpace(_strengthUIName) ? "Effect Power" : _strengthUIName;

        public string RadiusUIName => string.IsNullOrWhiteSpace(_radiusUIName) ? "Radius" : _radiusUIName;

        public string DurationUIName => string.IsNullOrWhiteSpace(_durationUIName) ? "Duration" : _durationUIName;

        #endregion
    }
}
