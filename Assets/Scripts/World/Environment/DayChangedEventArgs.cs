using System;
using World.Data;

namespace World.Environment
{
    public class DayChangedEventArgs : EventArgs
    {
        public DayTimeType DayTimeType { get; set; }
        public string Tag { get; set; }

        public DayChangedEventArgs(DayStateData so)
        {
            DayTimeType = so.DayTimeType;
            Tag = so.Tag;
        }
    }
}
