using Characters.Common.Abilities;
using Characters.Common.Combat;
using Characters.Common.Combat.Weapons;
using Characters.Common.Features;
using Characters.Common.Levels;
using World.Data;

namespace Characters.Common
{
    public interface ICharacterLogic : IEntityDynamicLogic
    {
        public EntityWeaponAbilitiesHandler WeaponsHandler { get; }
        public ICharacterLevel Level { get; }

        public void PerformBasicAttack(BasicAttack.LocalType type);
        public void ReactToDayStateChange(DayTimeType dayTime);
    }
}
