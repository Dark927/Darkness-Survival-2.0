using System;
using System.Collections.Generic;

namespace Settings.Global
{
    public abstract class CustomEventBase<TListener, TArgs> : ICustomEvent<TListener, TArgs>
        where TListener : IEventListener
        where TArgs : EventArgs
    {
        private List<TListener> _eventListeners;
        private bool _callingListeners;
        private Queue<Action> _delayedActions;


        public CustomEventBase()
        {
            _eventListeners = new List<TListener>();
            _callingListeners = false;
            _delayedActions = new Queue<Action>();
        }

        public abstract void EventRaiseAction(TListener listener, object sender, TArgs args);

        public virtual void RaiseEvent(object sender, TArgs args)
        {
            _callingListeners = true;

            foreach (var listener in _eventListeners)
            {
                EventRaiseAction(listener, sender, args);
            }

            _callingListeners = false;

            while (_delayedActions.Count > 0)
            {
                _delayedActions.Dequeue().Invoke();
            }
        }

        public void Subscribe(TListener listener)
        {
            if (!_callingListeners)
            {
                _eventListeners.Add(listener);
            }
            else
            {
                _delayedActions.Enqueue(() => _eventListeners.Add(listener));
            }
        }

        public void Unsubscribe(TListener listener)
        {
            if (!_callingListeners)
            {
                _eventListeners.Remove(listener);
            }
            else
            {
                _delayedActions.Enqueue(() => _eventListeners.Remove(listener));
            }
        }

        public virtual void Dispose()
        {
            _eventListeners.Clear();
        }
    }
}
