using Characters.Common.Combat.Weapons;
using Characters.Player.Animation;
using Characters.Player.Weapons;
using UnityEditor.Rendering.LookDev;
using UnityEngine.InputSystem;

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

            if (BasicAttack != null)
            {
                BasicAttack.ConfigureEventLinks();
            }
            else
            {
                OnBasicAttacksReady += ConfigureBasicAttacksEventListener;
            }

            _hasConfiguredLinks = true;
        }

        private void ConfigureBasicAttacksEventListener(BasicAttack attack)
        {
            OnBasicAttacksReady -= ConfigureBasicAttacksEventListener;
            attack.ConfigureEventLinks();
        }

        public override void RemoveEventLinks()
        {
            if (!_hasConfiguredLinks)
            {
                return;
            }

            base.RemoveEventLinks();
            BasicAttack?.RemoveEventLinks();

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

        #endregion
    }
}

