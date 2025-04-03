using System;
using Settings.Global;
using World.Environment;

namespace Settings
{
    public class DayStateEvent : CustomEventBase<IEventListener, DayChangedEventArgs>
    {
        public override void EventRaiseAction(IEventListener listener, object sender, DayChangedEventArgs args)
        {
            listener.Listen(sender, args);
        }
    }
}
