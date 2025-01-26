
using Characters.Interfaces;
using Characters.Stats;

public interface IEnemyLogic : IAttackable
{
    public CharacterStats Stats { get; }

}
