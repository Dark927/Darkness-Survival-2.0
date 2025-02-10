using Characters.Stats;

namespace Characters.Interfaces
{
    public interface IEntityLogic
    {
        public CharacterBodyBase Body { get; }
        public CharacterBaseData Data { get; }
        public CharacterStats Stats { get; }
    }
}
