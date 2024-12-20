﻿using Characters.Enemy.Data;
using Settings;
using System;
using UnityEngine;

namespace World.Components
{
    public class EnemyPool : ObjectPoolBase<GameObject>
    {
        #region Fields

        private EnemyData _enemyData;

        #endregion


        #region Methods

        #region Init

        public EnemyPool(EnemyData data, int preloadCount = ObjectPoolSettings.WRONG_PRELOAD_COUNT) : 
            base(() => PreloadFunc(data), RequestAction, ReturnAction, preloadCount)
        {
        }

        public EnemyPool(EnemyData data, Transform container, int preloadCount = ObjectPoolSettings.WRONG_PRELOAD_COUNT) : 
            base(() => PreloadFunc(data, container), RequestAction, ReturnAction, container, preloadCount)
        {
        }

        #endregion


        public static GameObject PreloadFunc(EnemyData data, Transform container = null)
        {
            GameObject createdObj = UnityEngine.Object.Instantiate(data.Prefab);
            createdObj.name = $"{data.Name} {data.Type}".Replace(" ", "_");

            if (container != null)
            {
                createdObj.transform.parent = container;
            }

            return createdObj;
        }

        public static void RequestAction(GameObject obj)
        {

            if(obj == null)
            {
                return;
            }

            // ToDo : remove this later, caller shoul set active state by his own.
            obj.SetActive(true);
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
