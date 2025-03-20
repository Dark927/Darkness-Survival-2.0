using System;
using Characters.Player.Levels;

namespace Characters.Common.Levels
{
    public interface ICharacterLevel : IEntityLevel
    {
        public float ActualXp { get; }
        public float XpProgressRatio { get; }
        public (float previous, float next) ActualXpBounds { get; }
        public float GainedXpFactor { get; }


        public event EventHandler<CharacterLevelArgs> OnUpdateXp;
        public void AddXp(int xp);
    }
}
