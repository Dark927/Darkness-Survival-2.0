using UnityEngine;

namespace Gameplay.Components
{
    public interface ICharacterConfigurator<TCharacter>
    {
        public void Configure(TCharacter character, Transform targetTransform = null);
        public void Deconfigure(TCharacter character);
    }
}