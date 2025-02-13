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
            BasicAttacks?.ConfigureEventLinks();

            _hasConfiguredLinks = true;
        }

        public override void RemoveEventLinks()
        {
            if (!_hasConfiguredLinks)
            {
                return;
            }

            base.RemoveEventLinks();
            BasicAttacks?.RemoveEventLinks();

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
            _animatorController = null;
        }

        #endregion

        #endregion
    }
}

