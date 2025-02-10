using Characters.Common.Combat.Weapons;
using Characters.Player.Weapons;

namespace Characters.Interfaces
{
    public interface IAttackable<out T> where T : BasicAttack
    {
        public CharacterWeaponsHolder Weapons { get; }
        public T BasicAttacks { get; }
    }
}