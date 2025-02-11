using Characters.Common;
using Characters.Interfaces;
using System.Diagnostics;
using World.Components.EnemyLogic;

public class EnemyController : EntityController
{
    #region Fields 

    private EnemyPool _targetPool;
    private IEnemyLogic _logic;

    #endregion


    #region Properties

    public EnemyPool TargetPool => _targetPool;

    #endregion


    #region Methods

    #region Init

    private void Awake()
    {
        _logic = GetComponentInChildren<IEnemyLogic>();
    }

    protected override void Start()
    {
        base.Start();
        _logic.Initialize();
    }

    protected override void OnEnable()
    {
        // We do not need to configure the enemy using OnEnable
        // just do that in the Enemy Configurator component.
    }

    public override void ConfigureEventLinks()
    {
        base.ConfigureEventLinks();

        _logic.ConfigureEventLinks();
        _logic.Body.OnBodyDies += OnCharacterDeath;
    }

    public override void RemoveEventLinks()
    {
        base.ConfigureEventLinks();

        _logic.Body.OnBodyDies -= OnCharacterDeath;
        _logic.RemoveEventLinks();
    }

    public void ResetCharacter()
    {
        _logic.ResetState();
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
