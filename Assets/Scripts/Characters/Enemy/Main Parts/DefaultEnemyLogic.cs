using Characters.Common;
using Characters.Common.Combat.Weapons;
using Characters.Interfaces;
using UnityEngine;

namespace Characters.Enemy
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class DefaultEnemyLogic : AttackableEntityLogic, IEnemyLogic, IAttackable<BasicAttack>
    {
        #region Fields 


        #endregion


        #region Properties


        #endregion


        #region Methods 

        #region Init

        protected override void InitBasicAttacks()
        {
            SetBasicAttacks(new BasicAttack(Body, Weapons.ActiveWeapons));
            base.InitBasicAttacks();
        }

        #endregion

        #endregion
    }
}
