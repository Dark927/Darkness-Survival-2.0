using Characters.Common;
using UnityEngine;

namespace Gameplay.Components
{
    public interface ICharacterConfigurator<TCharacter>
    {
        public void ConfigureCompletely(TCharacter character, ICharacterLogic targetEnemyCharacter = null);
        public void DeconfigureCompletely(TCharacter character);
    }
}
