using System;

namespace Gameplay.Components
{
    public class GameEventArgs : EventArgs
    {
        private GameStateEventType _eventType;

        public GameStateEventType EventType => _eventType;

        public GameEventArgs(GameStateEventType type)
        {
            _eventType = type;
        }
    }
}
