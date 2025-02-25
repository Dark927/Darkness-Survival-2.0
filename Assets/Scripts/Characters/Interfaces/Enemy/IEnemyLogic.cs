using Gameplay.Components.Items;
using UnityEngine;

namespace Characters.Interfaces
{
    public interface IEnemyLogic : IEntityDynamicLogic
    {
        public void SpawnRandomDropItem();
    }
}
