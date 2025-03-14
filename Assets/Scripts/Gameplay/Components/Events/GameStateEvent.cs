
using System;
using Settings.Global;

namespace Gameplay.Components
{
    public enum GameStateEventType
    {
        Unspecified = 0,
        StageStarted,
        StageStartFinishing,
        StagePaused,
        StageUnpaused,
        StageCompletelyOver,
        StageCompletelyPassed,
    }

    public class GameStateEvent : CustomEventBase<IEventListener, GameEventArgs>
    {
        public override void EventRaiseAction(IEventListener listener, object sender, GameEventArgs args)
        {
            listener.Listen(sender, args);
        }
    }
}
