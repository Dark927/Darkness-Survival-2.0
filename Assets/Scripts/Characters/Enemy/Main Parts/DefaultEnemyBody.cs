using Characters.Enemy.Data;
using Characters.Health;
using Characters.Player;
using Characters.TargetDetection;
using Cysharp.Threading.Tasks;
using Settings;
using System.Threading;
using UnityEngine;
using Zenject;

namespace Characters.Enemy
{
    public class DefaultEnemyBody : CharacterBody
    {
        #region Fields

        private IEnemyLogic _enemyLogic;
        private EnemyLookSideController _sideController;
        private EnemyBodyStats _bodyStats;

        private PlayerBody _targetPlayer;

        [Header("Target Detection")]
        [SerializeField] private TargetDetectionData _sideSwitchDetectionData;

        #endregion


        #region Properties

        public EnemyBodyStats Stats => _bodyStats;
        public PlayerBody TargetPlayer => _targetPlayer;

        #endregion


        #region Methods

        #region Init

        protected override void Init()
        {
            _enemyLogic = GetComponent<IEnemyLogic>();

            // ToDo : Get player for another place. Maybe create global singletone to save all players (?)
            _targetPlayer = FindObjectOfType<PlayerBody>();

            // ToDo : Do not use singleton maybe (?)
            _bodyStats = GlobalEnemyData.Instance.EnemySettings.BodyStats;
            InitSideController();

            Visual = GetComponentInChildren<EnemyVisual>();
            Health = new CharacterHealth(_enemyLogic.Stats.Health);
            Invincibility = new CharacterInvincibility(Visual.Renderer, _enemyLogic.Stats.InvincibilityTime, _enemyLogic.Stats.InvincibilityColor);
        }

        private void InitSideController()
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
            Movement = new EnemyMovement(this, _targetPlayer);
            CharacterSpeed speed = new CharacterSpeed() { MaxSpeedMultiplier = _enemyLogic.Stats.Speed };
            speed.SetMaxSpeedMultiplier();
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



        // Debug 

        private void OnDrawGizmosSelected()
        {
            InitSideController();
            _sideController.ShowDebug();
        }

        #endregion
    }
}