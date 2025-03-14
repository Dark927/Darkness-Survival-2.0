using System;

namespace Characters.Player
{
    public class PlayerEventArgs : EventArgs
    {
        private PlayerEvent.Type _eventType;

        public PlayerEvent.Type EventType => _eventType;

        public PlayerEventArgs(PlayerEvent.Type type)
        {
            _eventType = type;
        }
    }
}
