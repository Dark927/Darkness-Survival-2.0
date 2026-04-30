using Characters.Common;
using UnityEngine;

namespace Characters.Enemy
{
    public interface IEnemyLogic : IEntityDynamicLogic
    {
        public void SetDropItemContainer(Transform container);
        public void SpawnRandomDropItem();
        public void SetTarget(Transform targetTransform);
    }
}
