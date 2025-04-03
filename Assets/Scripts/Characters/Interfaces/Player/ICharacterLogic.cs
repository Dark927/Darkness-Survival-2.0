using System;
using Characters.Common.Combat;
using Characters.Common.Combat.Weapons;
using Characters.Common.Levels;
using World.Data;

namespace Characters.Interfaces
{
    public interface ICharacterLogic : IEntityDynamicLogic
    {
        public event Action<BasicAttack> OnBasicAttacksReady;
        public EntityWeaponsHolder Weapons { get; }
        public ICharacterLevel Level { get; }

        public void PerformBasicAttack(BasicAttack.LocalType type);
        public void ReactToDayStateChange(DayTimeType dayTime);
    }
}
