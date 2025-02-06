
using Characters.Health;
using Characters.Interfaces;
using Characters.Player.Attacks;
using Characters.Stats;

public interface ICharacterLogic : IAttackable
{
    public CharacterBodyBase Body { get; }
    public CharacterBasicAttack BasicAttacks { get; }
    public CharacterBaseData Data { get; }
    public CharacterStats Stats { get; }
}
