

using System;

namespace Settings.Global
{
    public interface ICustomEvent<TListener, TEventArgs> : IDisposable
        where TListener : IEventListener
        where TEventArgs : EventArgs
    {
        public void Subscribe(TListener listener);
        public void Unsubscribe(TListener listener);
        public void ListenEvent(object sender, TEventArgs args);
    }
}
