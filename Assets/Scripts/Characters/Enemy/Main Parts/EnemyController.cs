using Characters.Common;
using Characters.Interfaces;
using World.Components.EnemyLogic;
using Cysharp.Threading.Tasks;

public class EnemyController : EntityControllerBase
{
    #region Fields 

    private EnemyPool _targetPool;

    #endregion


    #region Properties

    public EnemyPool TargetPool => _targetPool;

    #endregion


    #region Methods

    #region Init

    protected override void Awake()
    {
        base.Awake();

        EntityLogic.Initialize();
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

    #endregion
}
