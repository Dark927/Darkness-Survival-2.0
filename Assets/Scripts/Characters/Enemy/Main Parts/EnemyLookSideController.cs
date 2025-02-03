using Characters.Player;
using Characters.TargetDetection;
using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Unity.VisualScripting;

namespace Characters.Enemy
{
    public sealed class EnemyLookSideController
    {
        private UniTask _activeSideSwitch;
        private CancellationTokenSource _cancellationTokenSource;

        private readonly DefaultEnemyBody _body;
        private readonly TargetDetector _targetDetector;

        private int _sideSwitchDelayInMs;
        private int _accelerationTimeInMs;

        private bool _isWaiting = false;


        #region Properties

        public bool Waiting { get => _isWaiting; private set => _isWaiting = value; }

        #endregion


        public EnemyLookSideController(DefaultEnemyBody enemyBody, TargetDetector detector, int sideSwitchDelayInMs, int accelerationTimeInMs)
        {
            _body = enemyBody;
            _targetDetector = detector;
            _sideSwitchDelayInMs = sideSwitchDelayInMs;
            _accelerationTimeInMs = accelerationTimeInMs;
        }

        public void TrySwitchSide()
        {
            bool needSideSwitch = !_body.View.IsLookingForward(_body.Movement.Direction)
                && !_targetDetector.IsTargetFoundOnVerticalAxis(_body.TargetTransform);

            if (needSideSwitch)
            {
                RequestSideSwitch();
            }
        }

        public void ShowDebug()
        {
            _targetDetector.IsTargetFoundOnVerticalAxis<PlayerBody>();
        }

        private void RequestSideSwitch()
        {
            if (_activeSideSwitch.Status == UniTaskStatus.Pending)
            {
                InterruptCurrentSideSwitch();
            }
            else
            {
                _activeSideSwitch = SideSwitch(_sideSwitchDelayInMs);
            }
        }

        private async UniTask SideSwitch(int delayInMs)
        {
            Waiting = true;
            _body.Movement.BlockMovement(delayInMs);

            _cancellationTokenSource = new CancellationTokenSource();
            CancellationToken token = _cancellationTokenSource.Token;

            await UniTask.Delay(delayInMs, cancellationToken: token);

            Waiting = false;

            if (token.IsCancellationRequested)
            {
                return;
            }

            _body.View.LookForward(_body.Movement.Direction);

            await _body.Movement.Speed.UpdateSpeedMultiplierLinear(_body.Movement.Speed.MaxSpeedMultiplier, _accelerationTimeInMs, token);
        }

        private void InterruptCurrentSideSwitch()
        {
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource?.Dispose();
            _cancellationTokenSource = null;
        }
    }
}
