using Characters.Common;
using UnityEngine;

namespace Characters.Enemy
{
    public interface IEnemyLogic : IEntityDynamicLogic
    {
        public void SpawnRandomDropItem();
        public void SetTarget(Transform targetTransform);
    }
}
