using Characters.Common;
using Characters.Common.Movement;
using Characters.Enemy.Data;
using Characters.Health;
using Characters.Interfaces;
using UnityEngine;
using Gameplay.Components.TargetDetection;

namespace Characters.Enemy
{
    public class DefaultEnemyBody : EntityPhysicsBodyBase
    {
        #region Fields

        private IEnemyLogic _enemyLogic;
        private EnemyLookSideController _sideController;
        private EnemyBodyStats _bodyStats;
        private EnemyMovement _enemyMovement;

        private Transform _targetTransform;

        [Header("Target Detection")]
        [SerializeField] private TargetDetectionData _sideSwitchDetectionData;

        #endregion


        #region Properties

        public EnemyBodyStats Stats => _bodyStats;
        public Transform TargetTransform => _targetTransform;
        public new EnemyMovement Movement => _enemyMovement;

        #endregion


        #region Methods

        #region Init

        protected override void InitComponents()
        {
            base.InitComponents();

            _enemyLogic = GetComponent<IEnemyLogic>();

            Visual = GetComponentInChildren<EnemyVisual>();
            Health = new EntityHealth(_enemyLogic.Stats.Health);
            Invincibility = new CharacterInvincibility(Visual.Renderer, _enemyLogic.Stats.InvincibilityTime, _enemyLogic.Stats.InvincibilityColor);

            _bodyStats = GlobalEnemyDataManager.Instance.RequestDefaultBodyStats();
            CreateSideController();
        }

        private void CreateSideController()
        {
            TargetDetector sideControlTargetDetector = new TargetDetector(transform, _sideSwitchDetectionData.Settings);
            _sideController = new EnemyLookSideController(this, sideControlTargetDetector, Stats.SideSwitchDelayInMs, Stats.AccelerationTimeInMs);
        }

        protected override void InitView()
        {
            View = new EntityLookDirection(transform);
        }

        protected override void InitMovement()
        {
            _enemyMovement = new EnemyMovement(transform);
            base.Movement = _enemyMovement;
            Movement.UpdateSpeedSettings(new SpeedSettings() { MaxSpeedMultiplier = _enemyLogic.Stats.Speed }, true);
        }

        public override void ConfigureEventLinks()
        {
            base.ConfigureEventLinks();

            OnBodyDamaged += Invincibility.Enable;
            OnBodyDies += Movement.Block;
        }

        public override void RemoveEventLinks()
        {
            base.RemoveEventLinks();

            OnBodyDamaged -= Invincibility.Enable;
            OnBodyDies -= Movement.Block;
        }

        public override void Dispose()
        {
            base.Dispose();
            _sideController?.Disable();
        }

        public override void ResetState()
        {
            base.ResetState();

            Invincibility?.Disable();
            Visual?.DeactivateActualColorBlink();
            Movement.UpdateSpeedSettings(new SpeedSettings() { MaxSpeedMultiplier = _enemyLogic.Stats.Speed }, true);
        }

        #endregion

        public void SetTarget(Transform targetTransform)
        {
            _targetTransform = targetTransform;
            Movement.SetTarget(targetTransform);
        }

        private void FixedUpdate()
        {
            if (_sideController == null || _sideController.Waiting)
            {
                return;
            }

            Movement.FollowTarget();

            if (Visual.IsVisibleForCamera)
            {
                _sideController.TrySwitchSide();
            }
        }


        #region Debug 

        private void OnDrawGizmosSelected()
        {
            try
            {
                CreateSideController();
                _sideController.ShowDebug();
            }
            catch
            {
                // # Just ignore this if we can not init side controller (to avoid error messages while configuring).
            }
        }

        #endregion

        #endregion
    }
}
