using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;


namespace Settings.Global
{
    public abstract class CustomEventBase<TListener, TArgs> : ICustomEvent<TListener, TArgs>
        where TListener : IEventListener
        where TArgs : EventArgs
    {
        private List<TListener> _eventListeners;
        private bool _callingListeners;


        public CustomEventBase()
        {
            _eventListeners = new List<TListener>();
            _callingListeners = false;
        }

        public abstract void EventRaiseAction(TListener listener, object sender, TArgs args);

        public virtual void ListenEvent(object sender, TArgs args)
        {
            _callingListeners = true;
            _eventListeners.ForEach(listener => EventRaiseAction(listener, sender, args));
            _callingListeners = false;
        }

        public void Subscribe(TListener listener)
        {
            HandleEventListenerAsync(() => _eventListeners?.Add(listener)).Forget();
        }

        public void Unsubscribe(TListener listener)
        {
            HandleEventListenerAsync(() => _eventListeners?.Remove(listener)).Forget();
        }

        public async UniTaskVoid HandleEventListenerAsync(Action callback)
        {
            await UniTask.WaitUntil(() => _callingListeners == false);
            callback?.Invoke();
        }

        public virtual void Dispose()
        {
            _eventListeners.Clear();
        }
    }
}
