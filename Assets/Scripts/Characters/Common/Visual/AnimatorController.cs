using Characters.Animation;
using UnityEngine;

namespace Characters.Common.Visual
{
    public class AnimatorController
    {
        #region Fields 

        private IAnimatorParameters _parameters;
        private IAnimationEvents _events;
        private Animator _animator;

        #endregion


        #region Properties

        public Animator Animator { get => _animator; private set => _animator = value; }
        public IAnimatorParameters Parameters { get => _parameters; private set => _parameters = value; }
        public IAnimationEvents Events { get => _events; protected set => _events = value; }

        #endregion


        #region Methods

        #region Init

        public AnimatorController(Animator characterAnimator, IAnimatorParameters parameters)
        {
            Animator = characterAnimator;
            Parameters = parameters;
        }

        public AnimatorController(Animator characterAnimator, IAnimatorParameters parameters, IAnimationEvents events)
        {
            Animator = characterAnimator;
            Parameters = parameters;
            Events = events;
        }

        public virtual void StopAnimation()
        {
            Animator.speed = 0;
        }

        public virtual void ResumeAnimation()
        {
            Animator.speed = 1;
        }

        #endregion

        #endregion
    }
}
