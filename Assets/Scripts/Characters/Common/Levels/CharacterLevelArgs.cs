using Characters.Common.Levels;


namespace Characters.Player.Levels
{
    public class CharacterLevelArgs : EntityLevelArgs
    {
        public (float previous, float next) ActualXpBounds { get; }
        public float XpProgressRatio { get; }

        public CharacterLevelArgs(int actualLevel, (float previous, float next) actualXpBounds, float xpProgressRatio) : base(actualLevel)
        {
            ActualXpBounds = actualXpBounds;
            XpProgressRatio = xpProgressRatio;
        }
    }
}
