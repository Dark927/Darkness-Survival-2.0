using System;

namespace Characters.Common.Levels
{
    public class EntityLevelArgs : EventArgs
    {
        public int ActualLevel { get; }

        public EntityLevelArgs(int actualLevel)
        {
            ActualLevel = actualLevel;
        }
    }
}
