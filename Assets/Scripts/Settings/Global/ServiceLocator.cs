using System;
using System.Collections.Generic;
using UnityEngine;

namespace Settings.Global
{
    public class ServiceLocator : IDisposable
    {
        #region Fields 

        private readonly Dictionary<string, IService> _services = new Dictionary<string, IService>();

        #endregion


        #region Properties

        public static ServiceLocator Current { get; private set; }

        #endregion

        #region Methods


        #region Init

        private ServiceLocator()
        {

        }

        public static void Initialize()
        {
            Current?.Dispose();
            Current = new ServiceLocator();
        }

        public void Dispose()
        {
            _services.Clear();
            Current = null;
        }

        #endregion

        /// <summary>
        /// Get registered service from Current service locator.
        /// </summary>
        /// <typeparam name="T">service type</typeparam>
        /// <returns>requested service</returns>
        public T Get<T>() where T : IService
        {
            string key = typeof(T).Name;

            if (!_services.ContainsKey(key))
            {
                Debug.LogError($"{key} not registered with {GetType().Name}");
                throw new InvalidOperationException();
            }

            return (T)_services[key];
        }

        /// <summary>
        /// Register new service for Current service locator.
        /// </summary>
        /// <typeparam name="T">service type</typeparam>
        /// <param name="service">service instance</param>
        public void Register<T>(T service) where T : IService
        {
            string key = typeof(T).Name;
            if (_services.ContainsKey(key))
            {
                Debug.LogError(
                    $"Attempted to register service of type {key} which is already registered with the {GetType().Name}.");
                return;
            }

            _services.Add(key, service);
        }

        /// <summary>
        /// Remove service from Current service locator.
        /// </summary>
        /// <typeparam name="T">service type</typeparam>
        public void Unregister<T>() where T : IService
        {
            string key = typeof(T).Name;
            TryUnregister(key);
        }

        public void Unregister(IService service)
        {
            string key = service.GetType().Name;

            TryUnregister(key);
        }
        private void TryUnregister(string key)
        {
            if (!_services.ContainsKey(key))
            {
                Debug.LogError(
                    $"Attempted to unregister service of type {key} which is not registered with the {GetType().Name}.");
                return;
            }

            _services.Remove(key);
        }

        #endregion
    }
}
