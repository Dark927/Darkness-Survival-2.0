using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Characters.Enemy.Animation
{
    public class EnemyAnimatorController : AnimatorController
    {
        #region Properties

        public new EnemyAnimatorParameters Parameters { get => base.Parameters as EnemyAnimatorParameters; }

        public float Speed
        {
            get => Animator.GetFloat(Parameters.SpeedFieldName);
            set => Animator.SetFloat(Parameters.SpeedFieldName, value);
        }

        #endregion


        #region Methods

        #region Init
        public EnemyAnimatorController(Animator characterAnimator, EnemyAnimatorParameters parameters) : base(characterAnimator, parameters)
        {
        }

        #endregion

        #endregion
    }
}