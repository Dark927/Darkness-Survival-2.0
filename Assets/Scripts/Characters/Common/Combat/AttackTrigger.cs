using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;

namespace Characters.Common.Combat
{
    public enum TriggerType
    {
        Enter = 1,
        Stay = 2,
        Exit = 3,
    }

    [RequireComponent(typeof(Collider2D))]
    public class AttackTrigger : MonoBehaviour,  IDisposable
    {
        #region Fields 

        private Collider2D _collider;
        private UniTask _currentTriggerActivation;
        private CancellationTokenSource _cancellationTokenSource;

        #endregion


        #region Events

        public event EventHandler OnTriggerEnter;
        public event EventHandler OnTriggerStay;
        public event EventHandler OnTriggerExit;

        #endregion


        #region Methods

        #region Init

        private void Awake()
        {
            _collider = GetComponent<Collider2D>();
            _currentTriggerActivation = UniTask.CompletedTask;

            if (!_collider.isTrigger)
            {
                _collider.isTrigger = true;
            }

            Deactivate();
        }

        public void Dispose()
        {
            CancelActiveTask();
        }

        #endregion

        public void Activate()
        {
            _collider.enabled = true;
        }

        public void Activate(float timeInSec)
        {
            if(_currentTriggerActivation.Status == UniTaskStatus.Pending)
            {
                CancelActiveTask();
            }

            Activate();
            _cancellationTokenSource = new CancellationTokenSource();
            _currentTriggerActivation = DeactivationDelay(timeInSec, _cancellationTokenSource.Token);
        }

        private async UniTask DeactivationDelay(float timeInSec, CancellationToken token)
        {
            await UniTask.WaitForSeconds(timeInSec, cancellationToken : token);

            Deactivate();
        }

        public void Deactivate()
        {
            _collider.enabled = false;
        }

        protected virtual void OnTriggerEnter2D(Collider2D collision)
        {
            InvokeTrigger(TriggerType.Enter, this, new AttackTriggerArgs(collision));
        }

        protected virtual void OnTriggerStay2D(Collider2D collision)
        {
            InvokeTrigger(TriggerType.Stay, this, new AttackTriggerArgs(collision));
        }

        protected virtual void OnTriggerExit2D(Collider2D collision)
        {
            InvokeTrigger(TriggerType.Exit, this, new AttackTriggerArgs(collision));
        }

        protected void InvokeTrigger(TriggerType type, object sender, EventArgs args)
        {
            EventHandler targetEvent = GetEvent(type);
            targetEvent?.Invoke(sender, args);
        }

        private EventHandler GetEvent(TriggerType type)
        {
            return type switch
            {
                TriggerType.Enter => OnTriggerEnter,
                TriggerType.Stay => OnTriggerStay,
                TriggerType.Exit => OnTriggerExit,
                _ => throw new InvalidOperationException()
            };
        }

        private void CancelActiveTask()
        {
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource?.Dispose();
            _cancellationTokenSource = null;
        }

        #endregion
    }
}
