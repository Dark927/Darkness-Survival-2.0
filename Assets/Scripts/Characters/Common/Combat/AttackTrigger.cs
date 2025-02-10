using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Utilities;

namespace Characters.Common.Combat
{
    public enum TriggerType
    {
        Enter = 1,
        Stay = 2,
        Exit = 3,
    }

    [RequireComponent(typeof(Collider2D))]
    public class AttackTrigger : MonoBehaviour, IDisposable
    {
        #region Fields 

        [Header("Trigger Settings")]
        [SerializeField] private bool _copyBodyColliderSettings = false;

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
            _currentTriggerActivation = UniTask.CompletedTask;

            _collider = GetComponent<Collider2D>();
            TrySetBodyColliderSettings();

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

        public void ScaleTriggerCollider(float scaleMultiplier)
        {
            TryScaleCollider(scaleMultiplier);
        }

        public void Activate()
        {
            _collider.enabled = true;
        }

        public void Activate(float timeInSec)
        {
            if (_currentTriggerActivation.Status == UniTaskStatus.Pending)
            {
                CancelActiveTask();
            }

            Activate();
            _cancellationTokenSource = new CancellationTokenSource();
            _currentTriggerActivation = DeactivationDelay(timeInSec, _cancellationTokenSource.Token);
        }

        private async UniTask DeactivationDelay(float timeInSec, CancellationToken token)
        {
            await UniTask.WaitForSeconds(timeInSec, cancellationToken: token);

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

        private void TrySetBodyColliderSettings()
        {
            if (!_copyBodyColliderSettings)
            {
                return;
            }

            CharacterBodyBase characterBody = GetComponentInParent<CharacterBodyBase>();

            if ((characterBody != null) && characterBody.TryGetComponent(out Collider2D bodyCollider))
            {
                _collider.CopyPropertiesFrom(bodyCollider);
            }
            else
            {
                Debug.LogWarning("# Can not find the body collider!");
            }
        }

        private void TryScaleCollider(float scaleMultiplier)
        {
            if (scaleMultiplier <= 0)
            {
                return;
            }

            _collider.ScaleCollider2D(scaleMultiplier);
        }

        #endregion
    }
}
