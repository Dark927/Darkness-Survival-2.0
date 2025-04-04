using System;
using Characters.Common;
using Characters.Common.Settings;
using Characters.Enemy.Settings;
using Cysharp.Threading.Tasks;
using Gameplay.Components;
using Gameplay.Components.Enemy;
using Settings.Global;

namespace Characters.Enemy
{
    public class EnemyController : EntityControllerBase, IEventListener
    {
        #region Fields 

        private EnemySpawner _targetSpawner;
        private EnemyData _enemyData;
        private IEnemyLogic _enemyLogic;

        #endregion


        #region Properties

        public EnemySpawner TargetSpawner => _targetSpawner;
        public EnemyData Data => _enemyData;
        public IEnemyLogic EnemyLogic => _enemyLogic;

        #endregion


        #region Methods

        #region Init

        public override void Initialize(IEntityData data)
        {
            base.Initialize(data);
            _enemyData = data as EnemyData;
            _enemyLogic = EntityLogic as IEnemyLogic;

            EntityLogic.Initialize(Data);
            InitFeaturesAsync().Forget();
        }

        protected override void OnEnable()
        {
            // We do not need to configure the enemy using OnEnable
            // just do that in the Enemy Configurator component.
        }

        protected override void OnDisable()
        {
            // We do not need to deconfigure the enemy using OnDisable
            // just do that in the Enemy Configurator component.
        }

        public override void ConfigureEventLinks()
        {
            base.ConfigureEventLinks();

            EntityLogic.ConfigureEventLinks();
            EntityLogic.Body.OnBodyDies += CharacterDeathListener;
        }

        public override void RemoveEventLinks()
        {
            base.RemoveEventLinks();

            EntityLogic.Body.OnBodyDies -= CharacterDeathListener;
            EntityLogic.RemoveEventLinks();
        }

        public void ResetCharacter()
        {
            EntityLogic.ResetState();
            _targetSpawner = null;
        }

        #endregion


        public void CharacterDeathListener()
        {
            EnemyLogic.SpawnRandomDropItem();
            RemoveEnemy();
        }

        public void SetTargetSpawner(EnemySpawner targetSpawner)
        {
            _targetSpawner = targetSpawner;
        }

        public void SetData(EnemyData data)
        {
            _enemyData = data;
        }

        public void Listen(object sender, EventArgs e)
        {
            switch (sender)
            {
                case GameStateService:
                    var args = e as GameEventArgs;
                    if (args.EventType == GameStateEventType.StageStartFinishing)
                    {
                        RemoveEnemy();
                    }
                    break;
            }


        }

        public void RemoveEnemy()
        {
            if (_targetSpawner != null)
            {
                _targetSpawner.ReturnEnemy(this);
            }
            else
            {
                print("destroy enemy");
                Destroy(gameObject);
            }
        }

        #endregion
    }
}
