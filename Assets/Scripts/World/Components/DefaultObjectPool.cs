using Settings;
using UnityEngine;

namespace World.Components
{
    public class DefaultObjectPool : ObjectPoolBase<GameObject>
    {
        public DefaultObjectPool(ObjectPoolData poolSettings, GameObject poolItem, int preloadCount = -1) : base(poolSettings, poolItem)
        {
            InitPool(preloadCount);
        }
    }
}
