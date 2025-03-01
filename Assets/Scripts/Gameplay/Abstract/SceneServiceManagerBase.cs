﻿using Assets.Scripts.Gameplay.Interfaces;
using Settings.Global;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Gameplay.Components
{
    public abstract class SceneServiceManagerBase : MonoBehaviour, ISceneServiceManager
    {
        #region Fields 

        private List<IService> _services = new List<IService>();
        private DiContainer _diContainer;

        #endregion


        #region Properties
        public List<IService> Services => _services;
        public DiContainer DiContainer => _diContainer;

        #endregion


        #region Methods

        #region Init

        [Inject]
        public void Construct(DiContainer diContainer)
        {
            _diContainer = diContainer;
        }

        #endregion

        protected virtual void OnDestroy()
        {
            Dispose();
        }

        public void Dispose()
        {
            foreach (var service in _services)
            {
                ServiceLocator.Current.Unregister(service);

                if (service is IDisposable disposableService)
                {
                    disposableService.Dispose();
                }
            }

            _services.Clear();
        }

        #endregion
    }
}
