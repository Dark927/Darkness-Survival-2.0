using System;
using Settings.Global;

namespace Settings
{
    public class CleanEvent : CustomEventBase<IEventListener, EventArgs>
    {
        public override void EventRaiseAction(IEventListener listener, object sender, EventArgs args)
        {
            listener.Listen(sender, args);
        }
    }
}
