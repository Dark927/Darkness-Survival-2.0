using Characters.Common;
using World.Components.EnemyLogic;
using Cysharp.Threading.Tasks;
using Characters.Enemy.Data;
using Characters.Stats;

public class EnemyController : EntityControllerBase
{
    #region Fields 

    private EnemyPool _targetPool;
    private EnemyData _enemyData;

    #endregion


    #region Properties

    public EnemyPool TargetPool => _targetPool;
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
        if (_targetPool != null)
        {
            _targetPool.ReturnObject(this.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetTargetPool(EnemyPool targetPool)
    {
        _targetPool = targetPool;
    }

    public void SetData(EnemyData data)
    {
        _enemyData = data;
    }

    #endregion
}
