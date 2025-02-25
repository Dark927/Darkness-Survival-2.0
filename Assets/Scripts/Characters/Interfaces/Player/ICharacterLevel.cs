

using Settings.Global;
using System;

namespace Characters.Common.Levels
{
    public interface ICharacterLevel : IEntityLevel
    {
        public float ActualXp { get; }
        public float XpProgressRatio { get; }
        public (float, float) ActualXpBounds { get; }
        public float GainedXpFactor { get; }


        public event Action<ICharacterLevel> OnUpdateXp;
        public void AddXp(int xp);
    }
}
