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
            Animator animator = GetComponent<Animator>();
            PlayerAnimController = new CharacterAnimatorController(animator, new PlayerAnimatorParameters());
        }

        public override AnimatorController GetAnimatorController()
        {
            return PlayerAnimController;
        }
    }
}