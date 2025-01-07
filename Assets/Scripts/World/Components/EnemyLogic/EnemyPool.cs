using Characters.Enemy.Data;
using Settings;
using System;
using UnityEngine;
using Zenject;

namespace World.Components.EnemyLogic
{
    public class EnemyPool : ObjectPoolBase<GameObject>
    {
        #region Fields

        #endregion


        #region Methods

        #region Init

        public EnemyPool(ObjectPoolSettings poolSettings,
                        EnemyData data,
                        int preloadCount = ObjectPoolSettings.NotIdentifiedPreloadCount) :
            base(poolSettings,
                () => PreloadFunc(data),
                RequestAction,
                ReturnAction,
                preloadCount)
        {
        }

        [Inject]
        public EnemyPool(ObjectPoolSettings poolSettings, 
                        EnemyData data, 
                        GameObjectsContainer container, 
                        int preloadCount = ObjectPoolSettings.NotIdentifiedPreloadCount) : 
            base(poolSettings, 
                () => PreloadFunc(data, container), 
                RequestAction, 
                ReturnAction, 
                container, 
                preloadCount)
        {
        }

        #endregion


        public static GameObject PreloadFunc(EnemyData data, GameObjectsContainer container = null)
        {
            GameObject createdObj = UnityEngine.Object.Instantiate(data.Prefab);
            createdObj.name = $"{data.Name} {data.Type}".Replace(" ", "_");

            if (container != null)
            {
                createdObj.transform.parent = container.transform;
            }

            return createdObj;
        }

        public static void RequestAction(GameObject obj)
        {
            if(obj == null)
            {
                return;
            }
        }

        public static void ReturnAction(GameObject obj)
        {
            if(obj == null)
            {
                throw new ArgumentNullException($"# Returning the null object to the object pool! - {nameof(EnemyPool)} : {nameof(ReturnAction)}");
            }

            obj.SetActive(false);
        }

        #endregion
    }
}
