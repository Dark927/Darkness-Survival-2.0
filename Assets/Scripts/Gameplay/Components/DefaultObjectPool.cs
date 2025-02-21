using Settings;
using UnityEngine;

namespace Gameplay.Components
{
    public class DefaultObjectPool : ObjectPoolBase<GameObject>
    {
        public DefaultObjectPool(ObjectPoolData poolSettings, GameObject poolItem, int preloadCount = -1) : base(poolSettings, poolItem)
        {
            InitPool(preloadCount);
        }
    }
}
