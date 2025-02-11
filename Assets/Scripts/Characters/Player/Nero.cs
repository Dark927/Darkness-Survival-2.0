using Characters.Player.Animation;
using Characters.Player.Weapons;

namespace Characters.Player
{
    public class Nero : PlayerCharacterLogic
    {
        #region Fields

        private CharacterAnimatorController _animatorController;
        private bool _hasConfiguredLinks = false;

        #endregion


        #region Properties


        #endregion


        #region Methods

        #region Init

        protected override void InitBasicAttacks()
        {
            SetBasicAttacks(new NeroBasicAttacks(this, Weapons.ActiveWeapons));
            base.InitBasicAttacks();
        }

        public override void ConfigureEventLinks()
        {
            if (_hasConfiguredLinks)
            {
                return;
            }

            base.ConfigureEventLinks();

            // -----------
            // # This logic can be unique for each character, so we do not subscribe inside the player body.
            // -----------

            BasicAttacks.OnAnyAttackStarted += Body.Movement.Block;

            // ToDo : CONFLICTS WITH PlayerDeath event!!!
            BasicAttacks.OnAttackFinished += Body.Movement.Unblock;

            // -----------

            _hasConfiguredLinks = true;
        }

        public override void RemoveEventLinks()
        {
            if (!_hasConfiguredLinks)
            {
                return;
            }

            base.RemoveEventLinks();
            BasicAttacks.OnAnyAttackStarted -= Body.Movement.Block;
            BasicAttacks.OnAttackFinished -= Body.Movement.Unblock;

            _hasConfiguredLinks = false;
        }

        protected override void SetReferences()
        {
            base.SetReferences();
            _animatorController = Body.Visual.GetAnimatorController() as CharacterAnimatorController;
        }

        protected override void Dispose()
        {
            base.Dispose();
            RemoveEventLinks();
            BasicAttacks?.Dispose();
            _animatorController = null;
        }

        #endregion

        #endregion
    }
}

