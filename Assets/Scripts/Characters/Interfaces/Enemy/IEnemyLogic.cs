using Gameplay.Components;
using UnityEngine;

namespace Characters.Interfaces
{
    public interface IEnemyLogic : IEntityDynamicLogic
    {
        public void SpawnRandomDropItem();
        public void SetTarget(Transform targetTransform);
    }
}
