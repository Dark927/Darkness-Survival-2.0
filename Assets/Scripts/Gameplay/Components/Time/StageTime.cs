
using System;
using UnityEngine;

namespace Gameplay.Components
{
    [System.Serializable]
    public struct StageTime : IComparable<StageTime>
    {
        #region Fields 

        public static readonly StageTime Zero = new StageTime() { Minutes = 0, Seconds = 0 };

        public uint Minutes;

        [Range(0, 59)]
        public uint Seconds;

        #endregion

        #region Operators

        public static bool operator >=(StageTime first, StageTime second)
        {
            return first.CompareTo(second) >= 0;
        }

        public static bool operator <=(StageTime first, StageTime second)
        {
            return first.CompareTo(second) <= 0;
        }

        public static implicit operator uint(StageTime time)
        {
            return ConvertToSeconds(time);
        }

        #endregion


        #region Methods

        public override string ToString()
        {
            return $"{Minutes.ToString("00")}:{Seconds.ToString("00")}";
        }

        public readonly int CompareTo(StageTime time)
        {
            uint sourceSeconds = ConvertToSeconds(this);
            uint argumentSeconds = ConvertToSeconds(time);

            return sourceSeconds.CompareTo(argumentSeconds);
        }

        public static uint ConvertToSeconds(StageTime time)
        {
            return (time.Minutes * 60) + time.Seconds;
        }

        public bool TryUpdateSeconds(uint seconds)
        {
            if (seconds == Seconds)
            {
                return false;
            }

            UpdateSeconds(seconds);
            return true;
        }

        public void UpdateSeconds(uint seconds)
        {
            Seconds = seconds;
            ConfigureTime();
        }

        public bool TryUpdateMinutes(uint minutes)
        {
            if (minutes == Minutes)
            {
                return false;
            }

            UpdateMinutes(minutes);
            return true;
        }

        public void UpdateMinutes(uint minutes)
        {
            Minutes = minutes;
        }

        public void Reset()
        {
            UpdateSeconds(0);
            UpdateMinutes(0);
        }

        private void ConfigureTime()
        {
            while (Seconds >= 60)
            {
                UpdateMinutes(Seconds / 60);
                Seconds %= 60;
            }
        }

        #endregion
    }
}
