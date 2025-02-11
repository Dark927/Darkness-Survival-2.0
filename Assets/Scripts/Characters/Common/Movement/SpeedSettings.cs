
namespace Characters.Common.Movement
{
    public struct SpeedSettings
    {
        #region Fields 

        public static readonly SpeedSettings Zero = new SpeedSettings
        {
            MaxSpeedMultiplier = 0,
            CurrentSpeedMultiplier = 0,
        };

        #endregion


        #region Properties

        public float CurrentSpeedMultiplier { get; set; }
        public float MaxSpeedMultiplier { get; set; }

        #endregion

        public override string ToString()
        {
            return $"Speed Settings : " +
                $"MaxSpeedMultiplier -> {MaxSpeedMultiplier}, " +
                $"CurrentSpeedMultiplier -> {CurrentSpeedMultiplier}";
        }
    }
}
