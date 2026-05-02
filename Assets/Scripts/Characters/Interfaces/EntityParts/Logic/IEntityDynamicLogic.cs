using Characters.Common.Abilities;
using Characters.Common.Settings;

namespace Characters.Common
{
    /// <summary>
    /// Common interface for entities which have health, physics, movement, simple abilities
    /// </summary>
    public interface IEntityDynamicLogic : IEntityLogic, IStunnable
    {
        public IEntityPhysicsBody Body { get; }
        public EntityInfo Info { get; }
        public CharacterStats Stats { get; }
        public EntityPassiveAbilitiesHandler AbilitiesHandler { get; }

        public void Initialize(IEntityData data);
    }
}
