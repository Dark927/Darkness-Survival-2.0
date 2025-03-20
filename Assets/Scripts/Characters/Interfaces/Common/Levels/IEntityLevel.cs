

using System;

namespace Characters.Common.Levels
{
    public interface IEntityLevel
    {
        protected const int DefaultMinLevel = 1;
        protected const int DefaultMaxLevel = 100;

        #region Properties

        public int ActualLevel { get; }
        public int MaxLevel { get; }

        #endregion


        #region Events

        public event EventHandler<EntityLevelArgs> OnLevelUp;

        #endregion


        #region Methods 

        public void LevelUp(EntityLevelArgs args = null);
        public void ResetLevel();

        #endregion
    }
}
