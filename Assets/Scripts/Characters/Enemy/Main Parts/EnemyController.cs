using Characters.Common;
using Characters.Interfaces;
using World.Components.EnemyLogic;
using Cysharp.Threading.Tasks;

public class EnemyController : EntityController
{
    #region Fields 

    private EnemyPool _targetPool;
    private IEntityLogic _enemyLogic;

    #endregion


    #region Properties

    public EnemyPool TargetPool => _targetPool;

    #endregion


    #region Methods

    #region Init

    protected override void Awake()
    {
        base.Awake();

        // TODO : Remove this testing field later 
        _enemyLogic = EntityLogic;
        _enemyLogic.Initialize();
        InitFeaturesAsync().Forget();
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void OnEnable()
    {
        // We do not need to configure the enemy using OnEnable
        // just do that in the Enemy Configurator component.
    }

    public override void ConfigureEventLinks()
    {
        base.ConfigureEventLinks();

        _enemyLogic.ConfigureEventLinks();
        _enemyLogic.Body.OnBodyDies += OnCharacterDeath;
    }

    public override void RemoveEventLinks()
    {
        base.ConfigureEventLinks();

        _enemyLogic.Body.OnBodyDies -= OnCharacterDeath;
        _enemyLogic.RemoveEventLinks();
    }

    public void ResetCharacter()
    {
        _enemyLogic.ResetState();
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

    #endregion
}
