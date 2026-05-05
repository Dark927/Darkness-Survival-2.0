using System.Threading;
using Characters.Common.Combat.Weapons.Data;
using Cysharp.Threading.Tasks;

namespace Characters.Common.Combat.Weapons
{
    public abstract class UpgradableAutoWeaponBase : UpgradableWeaponBase, IAutoWeapon
    {
        private CancellationTokenSource _cancellationTokenSource;

        public override void Initialize(WeaponAttackDataBase attackData)
        {
            base.Initialize(attackData);

            Owner.Body.OnBodyDies += StopAttack;

            StartAttack();
        }

        public override void Dispose()
        {
            base.Dispose();
            Owner.Body.OnBodyDies -= StopAttack;
            CancelActiveTask();
        }

        private async UniTaskVoid AutoAttackLoop(CancellationToken token)
        {
            await UniTask.WaitForSeconds(UpgradedAttackSettings.ReloadTimeSec, cancellationToken: token);

            while (!token.IsCancellationRequested)
            {
                await PerformAttackPhase(token);

                await UniTask.WaitForSeconds(UpgradedAttackSettings.ReloadTimeSec, cancellationToken: token);
            }
        }

        protected virtual async UniTask PerformAttackPhase(CancellationToken token)
        {
            FireWeapon();
            await UniTask.WaitForSeconds(UpgradedAttackSettings.FullDurationTimeSec, cancellationToken: token);
        }

        // Every auto-weapon MUST define what happens when the timer hits zero
        protected abstract void FireWeapon();

        public void StartAttack()
        {
            // Start the async attack loop
            _cancellationTokenSource = new CancellationTokenSource();
            AutoAttackLoop(_cancellationTokenSource.Token).Forget();
        }
        public void StopAttack()
        {
            CancelActiveTask();
        }

        private void CancelActiveTask()
        {
            if (_cancellationTokenSource != null)
            {
                _cancellationTokenSource.Cancel();
                _cancellationTokenSource.Dispose();
                _cancellationTokenSource = null;
            }
        }
    }
}
