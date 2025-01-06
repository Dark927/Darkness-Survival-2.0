using Settings;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace World.Components
{
    public class ObjectPoolBase<T> : IDisposable where T : class
    {
        #region Fields 

        private Func<T> _preloadFunc;
        private Action<T> _requestAction;
        private Action<T> _returnAction;

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

        public ObjectPoolBase(Func<T> preloadFunc, Action<T> requestAction, Action<T> returnAction, int preloadCount = ObjectPoolSettings.NotIdentifiedPreloadCount)
        {
            try
            {
                InitSettings(preloadFunc, requestAction, returnAction);
                InitPool(preloadCount);
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
                Dispose();
            }
        }

        public ObjectPoolBase(Func<T> preloadFunc, Action<T> requestAction, Action<T> returnAction, GameObjectsContainer container, int preloadCount = ObjectPoolSettings.NotIdentifiedPreloadCount)
        {
            try
            {
                InitSettings(preloadFunc, requestAction, returnAction, container);
                InitPool(preloadCount);
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
                Dispose();
            }
        }

        private void InitSettings(Func<T> preloadFunc, Action<T> requestAction, Action<T> returnAction, GameObjectsContainer container = null)
        {
            // ToDo : Try to use Zenject instead of this
            _poolSettings = Resources.Load<GlobalGameConfig>(ResourcesPath.GlobalGameConfigPath).PoolsSettings;

            _container = container;
            InitActions(preloadFunc, requestAction, returnAction);
        }

        private void InitActions(Func<T> preloadFunc, Action<T> requestAction, Action<T> returnAction)
        {
            _preloadFunc = preloadFunc;
            _requestAction = requestAction;
            _returnAction = returnAction;
        }

        private void InitPool(int preloadCount)
        {
            preloadCount = (preloadCount == ObjectPoolSettings.NotIdentifiedPreloadCount) ? _poolSettings.PreloadInstancesCount : preloadCount;

            _objectsQueue = new Queue<T>(preloadCount);
            _activeObjects = new List<T>();

            for (int currentIndex = 0; currentIndex < preloadCount; currentIndex++)
            {
                if (!CanExtend)
                {
                    break;
                }
                ReturnObject(_preloadFunc());
            }
        }

        #endregion


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