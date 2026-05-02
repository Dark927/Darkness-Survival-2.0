using UnityEngine;

namespace Gameplay.Components
{
    public class ComponentsPoolBase<T> : ObjectPoolBase<T> where T : Component
    {
        private GameObject _itemPrefab;


        /// <summary>
        /// Default object pool constructor
        /// </summary>
        /// <param name="poolSettings">Pool settings for pool configuration</param>
        /// <param name="poolItem">Target pool item to store, if null - create empty item of type T</param>
        /// <param name="container">Parent container for created items, if null - items will be parent to the root gameObject</param>
        public ComponentsPoolBase(ObjectPoolSettings poolSettings, GameObject poolItemPrefab, Transform container)
        {
            base.SetSettings(poolSettings, container);
            _itemPrefab = poolItemPrefab;
        }

        protected override string GenerateDefaultItemName(T poolItem)
        {
            string targetItemName;

            if (poolItem != null)
            {
                targetItemName = $"{poolItem.gameObject.name}".Replace(" ", "_");
            }
            else
            {
                targetItemName = typeof(T).Name;
            }

            return targetItemName;
        }

        protected override void ReturnAction(T item)
        {

        }

        protected override T PreloadFunc(Transform container = null)
        {
            GameObject itemObj;
            T item;

            if (_itemPrefab != null)
            {
                itemObj = GameObject.Instantiate(_itemPrefab);
            }
            else
            {
                itemObj = new GameObject(typeof(T).Name, typeof(T));
            }

            item = itemObj.GetComponent<T>();
            itemObj.name = GenerateDefaultItemName(item);

            if (container != null)
            {
                itemObj.transform.SetParent(container, false);
            }

            return item;
        }
    }
}
