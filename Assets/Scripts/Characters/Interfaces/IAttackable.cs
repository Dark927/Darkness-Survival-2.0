using Characters.Common.Combat;
using Characters.Common.Combat.Weapons;

namespace Characters.Interfaces
{
    public interface IAttackable<out T> where T : BasicAttack
    {
        public EntityWeaponsHolder Weapons { get; }
        public T BasicAttacks { get; }
    }
}