using System;

namespace Settings
{
    public class PauseEventArgs : EventArgs
    {
        private PauseEvent.Type _eventType;

        public PauseEvent.Type EventType => _eventType;


        public PauseEventArgs(PauseEvent.Type type)
        {
            _eventType = type;
        }
    }
}
