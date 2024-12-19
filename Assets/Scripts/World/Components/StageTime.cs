
using UnityEngine;

namespace World.Components
{
    [System.Serializable]
    public struct StageTime
    {
        public static readonly StageTime Zero = new StageTime() { Minutes = 0, Seconds = 0 };

        public uint Minutes;

        [Range(0, 59)]
        public uint Seconds;

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
    }
}
