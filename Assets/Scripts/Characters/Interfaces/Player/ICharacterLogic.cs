using Characters.Common.Combat.Weapons;
using Characters.Player.Weapons;
using System;

namespace Characters.Interfaces
{
    public interface ICharacterLogic : IEntityLogic
    {
        public event Action<BasicAttack> OnBasicAttacksReady;
        public EntityWeaponsHolder Weapons { get; }

        public void PerformBasicAttack(BasicAttack.LocalType type);
    }
}
