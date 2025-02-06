using Characters.Player;
using Characters.TargetDetection;
using Cysharp.Threading.Tasks;
using System;
using System.Threading;

namespace Characters.Enemy
{
    public sealed class EnemyLookSideController
    {
        #region Fields 

        private UniTask _activeSideSwitch;
        private CancellationTokenSource _cancellationTokenSource;

        private readonly DefaultEnemyBody _body;
        private readonly TargetDetector _targetDetector;

        private int _sideSwitchDelayInMs;
        private int _accelerationTimeInMs;

        private bool _isWaiting = false;

        #endregion


        #region Properties

        public bool Waiting { get => _isWaiting; private set => _isWaiting = value; }

        #endregion


        #region Methods

        #region Init

        public EnemyLookSideController(DefaultEnemyBody enemyBody, TargetDetector detector, int sideSwitchDelayInMs, int accelerationTimeInMs)
        {
            _body = enemyBody;
            _targetDetector = detector;
            _sideSwitchDelayInMs = sideSwitchDelayInMs;
            _accelerationTimeInMs = accelerationTimeInMs;
        }

        public void Disable()
        {
            InterruptCurrentSideSwitch();
        }

        #endregion

        public void TrySwitchSide()
        {
            bool needSideSwitch = !_body.View.IsLookingForward(_body.Movement.Direction)
                && !_targetDetector.IsTargetFoundOnVerticalAxis(_body.TargetTransform);

            if (needSideSwitch)
            {
                RequestSideSwitch();
            }
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
            _body.Movement.Block(delayInMs);

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

        #region Debug 

        public void ShowDebug()
        {
            _targetDetector.IsTargetFoundOnVerticalAxis<PlayerCharacterBody>();
        }

        #endregion

        #endregion

    }
}
