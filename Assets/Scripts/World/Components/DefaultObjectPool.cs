using Settings;
using UnityEngine;

namespace World.Components
{
    public class DefaultObjectPool : ObjectPoolBase<GameObject>
    {
        public DefaultObjectPool(ObjectPoolSettings poolSettings, GameObject poolItem, int preloadCount = -1) : base(poolSettings, poolItem)
        {
            InitPool(preloadCount);
        }
    }
}
