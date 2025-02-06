using Characters.Common;
using Characters.Common.Combat.Weapons;
using Characters.Interfaces;
using UnityEngine;

namespace Characters.Enemy
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class DefaultEnemyLogic : AttackableCharacterBase, IEnemyLogic, IAttackable<BasicAttack>
    {
        #region Fields 


        #endregion


        #region Properties

        #endregion


        #region Methods 

        #region Init

        protected override void InitComponents()
        {
            base.InitComponents();
            SetBasicAttacks(new BasicAttack(Body, Weapons.ActiveWeapons));
        }

        #endregion



        #endregion
    }
}
