using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Gameplay.Components
{
    public abstract class ObjectPoolBase<T> : IDisposable where T : class
    {
        #region Fields 

        private ObjectPoolSettings _poolSettings;

        private Queue<T> _objectsQueue;
        private LinkedList<T> _activeObjects;

        private Transform _container;

        #endregion


        #region Properties

        public bool CanExtend => (_objectsQueue.Count + _activeObjects.Count) < _poolSettings.MaxPoolCapacity;
        public Transform Container => _container;

        #endregion


        #region Methods

        #region Init

        public void SetSettings(ObjectPoolSettings poolSettings, Transform container = null)
        {
            try
            {
                _poolSettings = poolSettings;
                _container = container;
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
                Dispose();
            }
        }

        public virtual void Initialize(int preloadCount = ObjectPoolSettings.NotIdentifiedPreloadCount)
        {
            preloadCount = (preloadCount == ObjectPoolSettings.NotIdentifiedPreloadCount) ? (_poolSettings.PreloadInstancesCount) : preloadCount;

            _objectsQueue = new Queue<T>(preloadCount);
            _activeObjects = new LinkedList<T>();
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
                ReturnItem(PreloadFunc(_container));
            }
        }

        #endregion

        #region Clear

        public void Dispose()
        {
            _objectsQueue = null;
            _activeObjects = null;
        }

        #endregion

        protected abstract T PreloadFunc(Transform container = null);

        public void ReturnItem(T item)
        {
            ReturnAction(item);
            _activeObjects.Remove(item);
            _objectsQueue.Enqueue(item);
        }


        public T RequestObject()
        {
            T obj = _objectsQueue.Count > 0 ? _objectsQueue.Dequeue() : TryPreloadElement();

            if (obj != null)
            {
                _activeObjects.AddLast(obj);
                RequestAction(obj);
            }

            return obj;
        }

        /// <summary>
        /// get the oldest active item from the pool if there are no inactive items.
        /// </summary>
        public T RequestObjectForce()
        {
            T obj = RequestObject();

            if ((obj == null) && (_activeObjects.Count > 0))
            {
                ReturnItem(_activeObjects.First());
                obj = RequestObject();
            }

            return obj;
        }


        protected virtual void RequestAction(T obj)
        {

        }

        protected virtual void ReturnAction(T item)
        {
            if (item == null)
            {
                throw new ArgumentNullException($"# Returning the null item to the object pool! - {nameof(ReturnAction)}");
            }
        }


        protected virtual string GenerateDefaultItemName(T poolItem)
        {
            return typeof(T).Name;
        }


        private T TryPreloadElement()
        {
            if (CanExtend)
            {
                T obj = PreloadFunc(_container);
                return obj;
            }
            return null;
        }
    }

    #endregion

}
