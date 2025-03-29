

namespace Characters.Player.Upgrades
{
    /// <summary>
    /// This empty interface used in the abstract UpgradeSO.
    /// We need to cast it to the concrete upgrade level (if we want to access all upgrades inside it).
    /// </summary>
    public interface IUpgradeLevelSO
    {
        public string Description { get; }
        public UpgradeVisualDataUI CustomUpgradeVisualDataUI { get; }
    }
}
