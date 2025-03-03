using Characters.Stats;

namespace Characters.Interfaces
{
    public interface IEntityDynamicLogic : IEntityLogic, IStunnable
    {
        public IEntityPhysicsBody Body { get; }
        public IEntityData Data { get; }
        public CharacterStats Stats { get; }

        public void Initialize(IEntityData data);
    }
}
