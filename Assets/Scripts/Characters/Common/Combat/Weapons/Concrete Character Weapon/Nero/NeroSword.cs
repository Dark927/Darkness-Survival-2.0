

using Characters.Common.Combat.Weapons.Data;
using Characters.Player;
using Characters.Player.Animation;
using Materials;

namespace Characters.Common.Combat.Weapons
{
    public class NeroSword : CharacterSword, IConcreteEntityWeapon<NeroLogic>
    {
        #region Fields 

        private NeroLogic _concreteOwner;
        NeroVisual _neroVisual;

        #endregion


        #region Properties 

        public NeroLogic ConcreteOwner => _concreteOwner;

        #endregion


        #region Methods

        public override void Initialize(WeaponAttackDataBase attackData)
        {
            base.Initialize(attackData);
            _concreteOwner = Owner as NeroLogic;
            _neroVisual = _concreteOwner.Body.Visual as NeroVisual;
        }

        public void ApplySpecialAura(ScriptableMaterialPropsBase auraFxData)
        {
            _neroVisual.SetAura(auraFxData);
        }

        #endregion
    }
}
