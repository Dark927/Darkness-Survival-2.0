using Characters.Common;
using Characters.Enemy.Data;
using Characters.Interfaces;
using Characters.Stats;
using Cysharp.Threading.Tasks;
using Gameplay.Components.Enemy;

public class EnemyController : EntityControllerBase
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

    public override void ConfigureEventLinks()
    {
        base.ConfigureEventLinks();

        EntityLogic.ConfigureEventLinks();
        EntityLogic.Body.OnBodyDies += OnCharacterDeath;
    }

    public override void RemoveEventLinks()
    {
        base.ConfigureEventLinks();

        EntityLogic.Body.OnBodyDies -= OnCharacterDeath;
        EntityLogic.RemoveEventLinks();
    }

    public void ResetCharacter()
    {
        EntityLogic.ResetState();
    }

    #endregion


    public void OnCharacterDeath()
    {
        if (_targetSpawner != null)
        {
            EnemyLogic.SpawnRandomDropItem();
            _targetSpawner.ReturnEnemy(this);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetTargetSpawner(EnemySpawner targetSpawner)
    {
        _targetSpawner = targetSpawner;
    }

    public void SetData(EnemyData data)
    {
        _enemyData = data;
    }

    #endregion
}
