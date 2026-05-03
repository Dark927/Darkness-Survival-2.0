
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
    public class AttackTrigger : MonoBehaviour, IAttackTrigger
    {
        #region Fields 

        [Header("Trigger Settings")]
        [SerializeField] private bool _copyBodyColliderSettings = false;

        private Collider2D _collider;
        private UniTask _currentTriggerActivation = UniTask.CompletedTask;
        private CancellationTokenSource _cancellationTokenSource;

        #endregion

        public float CurrentActivationTime { get; private set; }


        #region Events

        public event EventHandler OnTriggerEnter;
        public event EventHandler OnTriggerStay;
        public event EventHandler OnTriggerExit;
        public event Action<IAttackTrigger> OnTriggerDeactivation;
        public event Action<IAttackTrigger> OnTriggerActivation;

        #endregion


        #region Methods

        #region Init

        public void Initialize()
        {
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

        public void ResetTriggerCollider()
        {

        }

        public void Activate()
        {
            _collider.enabled = true;
            OnTriggerActivation?.Invoke(this);
        }

        public void Activate(float timeInSec)
        {
            if (_currentTriggerActivation.Status == UniTaskStatus.Pending)
            {
                CancelActiveTask();
            }

            CurrentActivationTime = timeInSec;

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
            OnTriggerDeactivation?.Invoke(this);
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

            IEntityBody characterBody = GetComponentInParent<IEntityBody>(true);

            if ((characterBody != null) && characterBody.Transform.TryGetComponent(out Collider2D bodyCollider))
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

        #region Debugging

        private void OnDrawGizmos()
        {
            Collider2D col = _collider != null ? _collider : GetComponent<Collider2D>();

            // Only draw the Gizmo if the collider exists AND is actively enabled (attacking)
            if (col != null && col.enabled)
            {
                // Semi-transparent red for danger
                Gizmos.color = new Color(1f, 0f, 0f, 0.4f);

                // Draw a box that perfectly matches the physical bounds of the collider
                Gizmos.DrawCube(col.bounds.center, col.bounds.extents * 2);

                Gizmos.color = Color.red;
                Gizmos.DrawWireCube(col.bounds.center, col.bounds.extents * 2);
            }
        }

        #endregion


    }
}
