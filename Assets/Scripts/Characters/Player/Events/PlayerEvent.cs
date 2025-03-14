using Settings.Global;

namespace Characters.Player
{
    public class PlayerEvent : CustomEventBase<IEventListener, PlayerEventArgs>
    {
        public enum Type
        {
            Unspecified = 0,
            Dies,
            Dead,
            Win,
        }

        public override void EventRaiseAction(IEventListener listener, object sender, PlayerEventArgs args)
        {
            listener.Listen(sender, args);
        }
    }
}
