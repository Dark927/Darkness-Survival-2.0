using System;
using System.Collections.Generic;
using Settings;
using UnityEngine;

namespace World.Components
{
    public abstract class ObjectPoolBase<T> : IDisposable where T : class
    {
        #region Fields 

        private T _poolItem;
        private ObjectPoolSettings _poolSettings;

        private Queue<T> _objectsQueue;
        private List<T> _activeObjects;

        private GameObjectsContainer _container;

        #endregion


        #region Properties

        public bool CanExtend => (_objectsQueue.Count + _activeObjects.Count) < _poolSettings.MaxPoolCapacity;
        public GameObjectsContainer Container => _container;

        #endregion


        #region Methods

        #region Init

        public ObjectPoolBase(ObjectPoolSettings poolSettings, T poolItem)
        {
            try
            {
                InitSettings(poolSettings, poolItem);
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
                Dispose();
            }
        }

        public ObjectPoolBase(ObjectPoolSettings poolSettings, T poolItem, GameObjectsContainer container)
        {
            try
            {
                InitSettings(poolSettings, poolItem, container);
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
                Dispose();
            }
        }


        private void InitSettings(ObjectPoolSettings poolSettings, T poolItem, GameObjectsContainer container = null)
        {
            _poolItem = poolItem;
            _poolSettings = poolSettings;
            _container = container;
        }

        protected virtual void InitPool(int preloadCount)
        {
            preloadCount = (preloadCount == ObjectPoolSettings.NotIdentifiedPreloadCount) ? (_poolSettings.PreloadInstancesCount) : preloadCount;

            _objectsQueue = new Queue<T>(preloadCount);
            _activeObjects = new List<T>();

            PreloadElements(preloadCount);
        }

        private void PreloadElements(int preloadCount)
        {
            for (int currentIndex = 0; currentIndex < preloadCount; currentIndex++)
            {
                if (!CanExtend)
                {
                    break;
                }
                ReturnObject(PreloadFunc(_poolItem, _container) as T);
            }
        }

        #endregion

        #region Clear

        public void Dispose()
        {
            _objectsQueue = null;
            _activeObjects = null;
            _poolSettings = null;
        }

        #endregion

        public void ReturnObject(T obj)
        {
            ReturnAction(obj as GameObject);
            _activeObjects.Remove(obj);
            _objectsQueue.Enqueue(obj);
        }

        public T RequestObject()
        {
            T obj = _objectsQueue.Count > 0 ? _objectsQueue.Dequeue() : TryPreloadElement();

            if (obj != null)
            {
                _activeObjects.Add(obj);
                RequestAction(obj);
            }

            return obj;
        }

        protected virtual GameObject PreloadFunc(T poolItem, GameObjectsContainer container = null)
        {
            GameObject prefab = poolItem as GameObject;

            GameObject createdObj = UnityEngine.Object.Instantiate(prefab);
            createdObj.name = $"{prefab.name}".Replace(" ", "_");

            if (container != null)
            {
                createdObj.transform.parent = container.transform;
            }

            return createdObj;
        }

        protected virtual void RequestAction(T obj)
        {

        }

        protected virtual void ReturnAction(GameObject obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException($"# Returning the null object to the object pool! - {nameof(ReturnAction)}");
            }
        }

        private T TryPreloadElement()
        {
            if (CanExtend)
            {
                T obj = PreloadFunc(_poolItem, _container) as T;
                return obj;
            }
            return null;
        }
    }

    #endregion

}