using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

namespace Characters.Enemy
{
    public class DefaultEnemyBody : CharacterBody
    {
        #region Fields

        [SerializeField] private int _sideSwitchDelayInMs = 0;
        [SerializeField] private int _accelerationTimeInMs = 500;

        private bool _isWaiting = false;

        private UniTask _activeSideSwitch;
        private CancellationTokenSource _cancellationTokenSource;

        private TargetDetector _targetDetector;
        private PlayerBody _targetPlayer;

        // ToDo : In the future these parameters must be in the scriptable object (Enemy data)

        [Header("Target Detection")]
        [SerializeField] private float _targetDetectionDistance;
        [SerializeField] private float _targetDetectionAreaWidth;

        #endregion


        #region Properties

        public bool Waiting { get => _isWaiting; private set => _isWaiting = value; }

        #endregion


        #region Methods

        #region Init

        protected override void Init()
        {
            // ToDo : Maybe move this logic to another place.
            _targetPlayer = FindObjectOfType<PlayerBody>();
            _targetDetector = new TargetDetector(transform);

            Visual = GetComponentInChildren<EnemyVisual>();
        }

        protected override void InitView()
        {
            View = new CharacterLookDirection(transform);
        }

        protected override void InitMovement()
        {
            Movement = new EnemyMovement(this, _targetPlayer);
            CharacterSpeed speed = new CharacterSpeed() { CurrentSpeedMultiplier = 3, MaxSpeedMultiplier = 3 };

            Movement.Speed.Set(speed);
        }

        protected override void InitReferences()
        {

        }

        protected override void ClearReferences()
        {

        }

        #endregion

        private void FixedUpdate()
        {
            if (Waiting)
            {
                return;
            }

            Movement.Move();

            if (!Visual.IsVisibleForCamera)
            {
                return;
            }

            bool needSideSwitch = !View.IsLookingForward(Movement.Direction)
                && !_targetDetector.IsTargetFoundOnVerticalAxis(_targetPlayer.transform.position, _targetDetectionDistance, _targetDetectionAreaWidth);

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
            Movement.BlockMovement(delayInMs);

            _cancellationTokenSource = new CancellationTokenSource();
            CancellationToken token = _cancellationTokenSource.Token;

            await UniTask.Delay(delayInMs, cancellationToken: token);

            Waiting = false;

            if (token.IsCancellationRequested)
            {
                return;
            }

            View.LookForward(Movement.Direction);

            await Movement.Speed.UpdateSpeedMultiplierLinear(Movement.Speed.MaxSpeedMultiplier, _accelerationTimeInMs, token);
        }

        private void InterruptCurrentSideSwitch()
        {
            _cancellationTokenSource?.Cancel();
        }


        // Debug 

        private void OnDrawGizmosSelected()
        {
            _targetDetector = new TargetDetector(transform);
            _targetDetector.IsTargetFoundOnVerticalAxis<PlayerBody>(_targetDetectionDistance, _targetDetectionAreaWidth);
        }

        #endregion

    }
}