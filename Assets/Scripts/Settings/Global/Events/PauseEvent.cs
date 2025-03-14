using Settings.Global;

namespace Settings
{
    public class PauseEvent : CustomEventBase<IEventListener, PauseEventArgs>
    {
        public enum Type
        {
            PauseRequested,
            UnpauseRequested,
            Paused,
            Unpaused,
        }

        public override void EventRaiseAction(IEventListener listener, object sender, PauseEventArgs args)
        {
            listener.Listen(sender, args);
        }
    }
}
