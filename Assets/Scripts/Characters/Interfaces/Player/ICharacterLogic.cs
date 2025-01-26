
using Characters.Health;
using Characters.Interfaces;
using Characters.Player.Attacks;
using Characters.Stats;

public interface ICharacterLogic : IAttackable
{
    public CharacterBody Body { get; }
    public CharacterBasicAttack BasicAttacks { get; }
    public CharacterStats Stats { get; }

}
