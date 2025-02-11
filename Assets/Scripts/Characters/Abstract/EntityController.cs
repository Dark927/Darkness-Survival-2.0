using Settings.Global;
using System;
using UnityEngine;

namespace Characters.Common
{
    public abstract class EntityController : MonoBehaviour, IEventListener, IDisposable
    {
        #region Fields 

        private bool _started = false;

        #endregion


        #region Methods

        #region Init

        protected virtual void Start()
        {
            _started = true;
        }

        protected virtual void OnEnable()
        {
            if (_started)
            {
                ConfigureEventLinks();
            }
        }

        protected virtual void OnDisable()
        {
            RemoveEventLinks();
        }

        private void OnDestroy()
        {
            Dispose();
        }

        public virtual void Dispose()
        {

        }

        #endregion

        public virtual void ConfigureEventLinks()
        {

        }

        public virtual void RemoveEventLinks()
        {

        }

        #endregion
    }
}
