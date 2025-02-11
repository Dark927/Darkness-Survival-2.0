using Characters.Common.Visual;
using Characters.Enemy.Animation;
using UnityEngine;

namespace Characters.Enemy
{
    public class EnemyVisual : EntityVisualBase
    {
        public EnemyAnimatorController AnimController { get; set; }

        public override void Initialize()
        {
            base.Initialize();
            InitAnimation();
        }

        private void InitAnimation()
        {
            Animator animator = GetComponent<Animator>();
            AnimController = new EnemyAnimatorController(animator, new EnemyAnimatorParameters());
        }

        public override AnimatorController GetAnimatorController() => AnimController;
        public override T GetAnimatorController<T>() => AnimController as T;
    }
}
