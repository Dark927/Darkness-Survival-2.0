using Settings;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace World.Components
{
    public class ObjectPoolBase<T> : IDisposable where T : class
    {
        #region Fields 

        private Queue<T> _objectsQueue;

        private Func<T> _preloadFunc;
        private Action<T> _requestAction;
        private Action<T> _returnAction;

        private ObjectPoolSettings _poolSettings;
        private List<T> _activeObjects;

        #endregion


        #region Properties

        public bool CanExtend => (_objectsQueue.Count + _activeObjects.Count) < _poolSettings.MaxPoolCapacity;

        #endregion


        #region Methods

        public ObjectPoolBase(Func<T> preloadFunc, Action<T> requestAction, Action<T> returnAction)
        {
            try
            {
                _preloadFunc = preloadFunc;
                _requestAction = requestAction;
                _returnAction = returnAction;

                _poolSettings = Resources.Load<GlobalGameConfig>("Data/Settings/GlobalGameConfig").PoolsSettings;

                InitPool();
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
                Dispose();
            }
        }

        private void InitPool()
        {
            int preloadCount = _poolSettings.PreloadInstancesCount;
            _objectsQueue = new Queue<T>(preloadCount);
            _activeObjects = new List<T>();

            for (int currentIndex = 0; currentIndex < preloadCount; currentIndex++)
            {
                ReturnObject(_preloadFunc());
            }
        }

        public void ReturnObject(T obj)
        {
            _returnAction(obj);
            _activeObjects.Remove(obj);
            _objectsQueue.Enqueue(obj);
        }

        public T RequestObject()
        {
            T obj = _objectsQueue.Count > 0 ? _objectsQueue.Dequeue() : TryPreloadElement();

            if (obj != null)
            {
                _activeObjects.Add(obj);
                _requestAction(obj);
            }

            return obj;
        }

        public void Dispose()
        {
            _objectsQueue = null;
            _activeObjects = null;
            _poolSettings = null;
        }

        private T TryPreloadElement()
        {
            if (CanExtend)
            {
                T obj = _preloadFunc();
                return obj;
            }
            return null;
        }
    }

    #endregion

}