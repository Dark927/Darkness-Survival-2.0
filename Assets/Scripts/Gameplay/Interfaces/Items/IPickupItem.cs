using Characters.Common;

namespace Gameplay.Components.Items
{
    public interface IPickupItem
    {
        public IItemParameters Parameters { get; }

        public void Pickup(ICharacterLogic targetCharacter);
        public void SetAllParameters(IItemParameters parameters);
    }
}
