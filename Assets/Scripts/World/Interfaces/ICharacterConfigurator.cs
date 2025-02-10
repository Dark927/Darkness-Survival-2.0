using UnityEngine;

namespace World.Components
{
    public interface ICharacterConfigurator<TCharacter>
    {
        public void Configure(TCharacter character, Transform targetTransform = null);
    }
}