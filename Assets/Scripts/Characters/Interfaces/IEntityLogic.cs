using Characters.Stats;
using Settings.Global;
using Unity.VisualScripting;

namespace Characters.Interfaces
{
    public interface IEntityLogic : IEventListener, IResetable, IInitializable
    {
        public IEntityBody Body { get; }
        public CharacterBaseData Data { get; }
        public CharacterStats Stats { get; }
    }
}
