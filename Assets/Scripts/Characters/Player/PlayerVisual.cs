using Characters.Player.Animation;
using UnityEngine;

namespace Characters.Player
{
    public class PlayerVisual : CharacterVisual
    {
        public CharacterAnimatorController PlayerAnimController { get; set; }

        protected override void Init()
        {
            base.Init();

            InitAnimation();
        }

        private void InitAnimation()
        {
            CharacterAnimationEvents animationEvents = GetComponent<CharacterAnimationEvents>();    
            Animator animator = GetComponent<Animator>();

            PlayerAnimController = new CharacterAnimatorController(animator, new CharacterAnimatorParameters(), animationEvents);
        }

        public override AnimatorController GetAnimatorController() => PlayerAnimController;
        public override T GetAnimatorController<T>() => PlayerAnimController as T;
    }
}