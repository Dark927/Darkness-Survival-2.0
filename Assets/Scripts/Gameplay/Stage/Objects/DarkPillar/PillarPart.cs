using Settings.Global;
using System;
using UnityEngine;

namespace Gameplay.Stage.Objects
{
    public class PillarPart : MonoBehaviour, IInitializable
    {
        #region Fields 

        private ActivatableAnimatorController _animatorController;

        #endregion


        #region Properties

        #endregion


        #region Methods

        #region Init

        public virtual void Initialize()
        {
            if (_animatorController == null)
            {
                Animator animator = GetComponent<Animator>();
                _animatorController = new ActivatableAnimatorController(animator, new ActivatableAnimatorParameters());
            }
        }

        #endregion

        public void Activate()
        {
            TryDoAction(() => _animatorController.TriggerActivation());
        }

        public void Deactivate()
        {
            TryDoAction(() => _animatorController.TriggerDeactivation());
        }

        private void TryDoAction(Action action)
        {
            if (_animatorController == null)
            {
                Debug.LogWarning($" # {nameof(PillarPart)} is not initialized");
                return;
            }

            action?.Invoke();
        }


        #endregion
    }
}
