using Characters.Enemy.Data;
using Characters.Health;
using Characters.TargetDetection;
using System;
using UnityEngine;

namespace Characters.Enemy
{
    public class DefaultEnemyBody : CharacterBodyBase
    {
        #region Fields

        private IEnemyLogic _enemyLogic;
        private EnemyLookSideController _sideController;
        private EnemyBodyStats _bodyStats;

        private Transform _targetTransform;

        [Header("Target Detection")]
        [SerializeField] private TargetDetectionData _sideSwitchDetectionData;

        #endregion


        #region Properties

        public EnemyBodyStats Stats => _bodyStats;
        public Transform TargetTransform => _targetTransform;

        #endregion


        #region Methods

        #region Init

        protected override void Init()
        {
            _enemyLogic = GetComponent<IEnemyLogic>();


            Visual = GetComponentInChildren<EnemyVisual>();
        }

        private void CreateSideController()
        {
            TargetDetector sideControlTargetDetector = new TargetDetector(transform, _sideSwitchDetectionData.Settings);
            _sideController = new EnemyLookSideController(this, sideControlTargetDetector, Stats.SideSwitchDelayInMs, Stats.AccelerationTimeInMs);
        }

        protected override void InitView()
        {
            View = new CharacterLookDirection(transform);
        }

        protected override void InitMovement()
        {
            Movement = new EnemyMovement(transform);
            CharacterSpeed speed = new CharacterSpeed() { MaxSpeedMultiplier = _enemyLogic.Stats.Speed };
            speed.SetMaxSpeedMultiplier();
            Movement.Speed.Set(speed);
        }

        protected override void Start()
        {
            base.Start();

            _bodyStats = GlobalEnemyData.Instance.RequestDefaultBodyStats();

            CreateSideController();
            Health = new CharacterHealth(_enemyLogic.Stats.Health);
            Invincibility = new CharacterInvincibility(Visual.Renderer, _enemyLogic.Stats.InvincibilityTime, _enemyLogic.Stats.InvincibilityColor);
        }

        protected override void InitReferences()
        {

        }

        public override void Dispose()
        {
            _sideController?.Dispose();
        }

        #endregion

        public void SetTarget(Transform targetTransform)
        {
            _targetTransform = targetTransform;
            (Movement as EnemyMovement).SetTarget(targetTransform);
        }

        private void FixedUpdate()
        {
            if (_sideController.Waiting)
            {
                return;
            }

            Movement.Move();

            if (Visual.IsVisibleForCamera)
            {
                _sideController.TrySwitchSide();
            }
        }

        public void TakeDamage()
        {

        }

        public void Heal()
        {

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