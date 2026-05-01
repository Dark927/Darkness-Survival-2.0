
using Characters.Common.Abilities;

namespace Characters.Player.Upgrades
{
    /// <summary>
    /// Common interface for all upgradable abilities
    /// </summary>
    public interface IUpgradableAbility : IUpgradable, IEntityAbility
    {
        /// <summary>
        /// Modifies the core power of the ability (Damage, Heal amount, Shield HP, etc.)
        /// </summary>
        /// <param name="percent">0 - 100</param>
        void ApplyStrengthUpgrade(float percent);

        /// <summary>
        /// Modifies the area of effect (AoE size, Explosion radius, etc.)
        /// </summary>
        /// <param name="percent">0 - 100</param>

        void ApplyRadiusUpgrade(float percent);

        /// <summary>
        /// Modifies the lifespan or active time of the ability
        /// </summary>
        /// <param name="percent">0 - 100</param>

        void ApplyDurationUpgrade(float percent);
    }
}
