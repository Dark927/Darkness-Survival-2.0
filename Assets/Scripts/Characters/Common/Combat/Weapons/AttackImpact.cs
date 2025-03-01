using Characters.Common.Physics2D;
using Cysharp.Threading.Tasks;
using Settings.CameraManagement.Shake;
using System.Threading;
using UnityEngine;

namespace Characters.Common.Combat
{
    public class AttackImpact
    {
        #region Fields

        private ShakeImpact _shake;
        private ImpactSettings _settings;
        private bool _isReady;
        private EntityPhysicsActions _physicsActions;

        private UniTask _currentReloadTask = UniTask.CompletedTask;
        private CancellationTokenSource _reloadCts;

        #endregion


        #region Properties

        public ShakeImpact Shake => _shake;
        public bool IsReady => _isReady;

        #endregion


        #region Methods 

        public AttackImpact(ImpactSettings settings)
        {
            _settings = settings;
            _shake = new ShakeImpact(settings.ShakeSettings);
            _physicsActions = new EntityPhysicsActions();
            _isReady = true;
        }

        public void SetShakeImpact(ShakeImpact shake)
        {
            _shake = shake;
        }

        public void ReloadImpact(bool forceReload = false)
        {
            if ((_currentReloadTask.Status == UniTaskStatus.Pending) && !forceReload)
            {
                return;
            }

            StopReload();
            _reloadCts = new CancellationTokenSource();
            _currentReloadTask = ReloadImpactAsync(_settings.ReloadTimeMs, _reloadCts.Token);
        }

        public void AddStun()
        {
            _physicsActions
                .AddStun(_settings.StunDurationMs);
        }

        public void AddKnockback(Vector2 direction)
        {
            _physicsActions
                .AddKnockback(_settings.PushForce, direction);
        }


        /// <summary>
        /// Calculates that impact can be used considering the existing chance.
        /// </summary>
        public bool CanUseRandomly()
        {
            return (UnityEngine.Random.Range(0, 100) <= _settings.ChancePercent);
        }

        public virtual void PerformPhysicsImpact(Collider2D targetCollider)
        {
            if (!targetCollider.TryGetComponent(out IEntityPhysics2D targetPhysics))
            {
                return;
            }

            targetPhysics.TryPerformPhysicsActions(_physicsActions);
        }

        private void StopReload()
        {
            if (_reloadCts == null)
            {
                return;
            }

            _reloadCts?.Cancel();
            _reloadCts?.Dispose();
            _reloadCts = null;
        }

        private async UniTask ReloadImpactAsync(int reloadTimeMs, CancellationToken token = default)
        {
            _isReady = false;
            await UniTask.Delay(reloadTimeMs, cancellationToken: token);
            _isReady = true;
        }

        #endregion
    }
}
