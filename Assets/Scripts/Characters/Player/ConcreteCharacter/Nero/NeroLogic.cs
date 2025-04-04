using Characters.Common.Combat.Weapons;
using Characters.Player.Animation;
using Characters.Player.Weapons;


namespace Characters.Player
{
    public class NeroLogic : PlayerCharacterLogic
    {
        #region Fields

        private CharacterAnimatorController _animatorController;
        private bool _hasConfiguredLinks = false;

        #endregion


        #region Properties


        #endregion


        #region Methods

        #region Init

        protected override BasicAttack GetBasicAttacks()
        {
            return new NeroBasicAttacks(this, WeaponsHandler.ActiveOnes);
        }

        public override void ConfigureEventLinks()
        {
            if (_hasConfiguredLinks)
            {
                return;
            }

            base.ConfigureEventLinks();

            _hasConfiguredLinks = true;
        }


        public override void RemoveEventLinks()
        {
            if (!_hasConfiguredLinks)
            {
                return;
            }

            base.RemoveEventLinks();
            WeaponsHandler?.RemoveEventLinks();

            _hasConfiguredLinks = false;
        }

        protected override void SetReferences()
        {
            base.SetReferences();
            _animatorController = Body.Visual.GetAnimatorController() as CharacterAnimatorController;
        }

        public override void Dispose()
        {
            base.Dispose();
            RemoveEventLinks();
            _animatorController = null;
        }

        #endregion

        public override void ListenNewWeaponGiven(object sender, IWeapon weapon)
        {
            base.ListenNewWeaponGiven(sender, weapon);

            if (weapon is CharacterSword weaponSword)
            {
                ((NeroBasicAttacks)WeaponsHandler.BasicAttacks).SetSword(weaponSword);
            }
        }

        #endregion
    }
}

