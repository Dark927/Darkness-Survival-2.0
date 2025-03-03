

using System;

namespace Characters.Common.Levels
{
    public interface IEntityLevel
    {
        public const int DefaultMinLevel = 1;
        public const int DefaultMaxLevel = 100;

        #region Properties

        public int ActualLevel { get; }
        public int MaxLevel { get; }

        #endregion


        #region Events

        public event Action<IEntityLevel> OnLevelUp;

        #endregion


        #region Methods 

        public void LevelUp();
        public void ResetLevel();

        #endregion
    }
}
