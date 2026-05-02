using System;
using Characters.Common.Abilities;

namespace Characters.Common
{
    public interface IAttackable
    {
        public event EventHandler<IEntityDynamicLogic> OnEnemyKilled;
        public EntityWeaponAbilitiesHandler WeaponsHandler { get; }

        public void NotifyEnemyKilled(IEntityDynamicLogic killedEnemy);
    }
}
