using System;
using Characters.Common.Combat;
using Characters.Common.Combat.Weapons;
using Characters.Common.Levels;
using Characters.Player.Upgrades;

namespace Characters.Interfaces
{
    public interface ICharacterLogic : IEntityDynamicLogic
    {
        public event Action<BasicAttack> OnBasicAttacksReady;
        public event Action<ICharacterLogic, EntityLevelArgs> OnReadyForUpgrade;

        public EntityWeaponsHolder Weapons { get; }
        public ICharacterLevel Level { get; }

        public void PerformBasicAttack(BasicAttack.LocalType type);
        public void ApplyUpgrade(UpgradeLevelSO<ICharacterLogic> upgrade);
    }
}
