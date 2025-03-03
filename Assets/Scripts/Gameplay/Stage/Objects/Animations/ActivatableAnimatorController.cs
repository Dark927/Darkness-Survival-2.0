using Characters.Common.Visual;
using UnityEngine;

namespace Gameplay.Stage.Objects
{
    public class ActivatableAnimatorController : AnimatorController
    {
        #region Properties

        public new ActivatableAnimatorParameters Parameters { get => base.Parameters as ActivatableAnimatorParameters; }

        #endregion


        #region Methods

        #region Init

        public ActivatableAnimatorController(Animator animator, ActivatableAnimatorParameters parameters) : base(animator, parameters)
        {

        }

        #endregion


        public void TriggerActivation() => Animator.SetTrigger(Parameters.ActivationFieldName);
        public void TriggerDeactivation() => Animator.SetTrigger(Parameters.DeactivationFieldName);

        #endregion
    }
}
