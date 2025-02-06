using Characters.Interfaces;
using Characters.Player.Animation;

namespace Characters.Player
{
    public class Nero : PlayerCharacterLogic
    {
        #region Fields

        private CharacterAnimatorController _animatorController;

        #endregion


        #region Properties


        #endregion


        #region Methods

        #region Init


        protected override void InitComponents()
        {
            base.InitComponents();
        }

        protected override void InitBasicAttacks()
        {
            base.InitBasicAttacks();
        }

        protected override void SetReferences()
        {
            base.SetReferences();
            _animatorController = Body.Visual.GetAnimatorController() as CharacterAnimatorController;

            // -----------
            // # This logic can be unique for each character, so we do not subscribe inside the player body.
            // -----------

            BasicAttacks.OnAnyAttackStarted += Body.Movement.Block;

            // ToDo : CONFLICTS WITH PlayerDeath event!!!
            BasicAttacks.OnAttackFinished += Body.Movement.Unblock;

            // -----------

            // ToDo : Move this logic to another place
            _animatorController.Events.OnDeathFinished += GameplayUI.Instance.ActivateGameOverPanel;
        }

        protected override void Dispose()
        {
            BasicAttacks.OnAnyAttackStarted -= Body.Movement.Block;
            BasicAttacks.OnAttackFinished -= Body.Movement.Unblock;
            BasicAttacks.Dispose();

            // ToDo : Move this logic to another place
            _animatorController.Events.OnDeathFinished -= GameplayUI.Instance.ActivateGameOverPanel;

            _animatorController = null;
        }

        #endregion

        #endregion
    }
}

