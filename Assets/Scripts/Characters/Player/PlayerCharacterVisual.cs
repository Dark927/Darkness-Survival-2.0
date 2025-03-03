using Characters.Common.Visual;
using Characters.Player.Animation;
using UnityEngine;

namespace Characters.Player
{
    public class PlayerCharacterVisual : EntityVisualBase
    {
        public CharacterAnimatorController PlayerAnimController { get; set; }

        public override void Initialize()
        {
            base.Initialize();

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
