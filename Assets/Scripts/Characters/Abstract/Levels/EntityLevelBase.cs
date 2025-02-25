using System;


namespace Characters.Common.Levels
{
    public abstract class EntityLevelBase : IEntityLevel
    {
        #region Fields

        private int _actualLevel = IEntityLevel.DefaultMinLevel;
        private int _maxLevel;

        #endregion


        #region Properties

        public virtual int ActualLevel => _actualLevel;
        public int MaxLevel => _maxLevel;

        #endregion



        #region Events

        public event Action<IEntityLevel> OnLevelUp;

        #endregion


        #region Methods 

        public EntityLevelBase(int targetMaxLevel = IEntityLevel.DefaultMaxLevel)
        {
            _maxLevel = targetMaxLevel;
        }

        public virtual void LevelUp()
        {
            OnLevelUp?.Invoke(this);
        }

        public void ResetLevel()
        {
            _actualLevel = IEntityLevel.DefaultMinLevel;
        }

        #endregion

    }
}
