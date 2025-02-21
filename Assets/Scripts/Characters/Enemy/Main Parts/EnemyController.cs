using Characters.Common;
using Gameplay.Components.Enemy;
using Cysharp.Threading.Tasks;
using Characters.Enemy.Data;
using Characters.Stats;

public class EnemyController : EntityControllerBase
{
    #region Fields 

    private EnemySpawner _targetSpawner;
    private EnemyData _enemyData;

    #endregion


    #region Properties

    public EnemySpawner TargetSpawner => _targetSpawner;
    public EnemyData Data => _enemyData;

    #endregion


    #region Methods

    #region Init

    public override void Initialize(IEntityData data)
    {
        base.Initialize(data);
        _enemyData = data as EnemyData;

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
