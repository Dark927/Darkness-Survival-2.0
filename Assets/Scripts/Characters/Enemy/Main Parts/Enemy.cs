using Characters.Interfaces;
using UnityEngine;
using World.Components.EnemyLogic;

public class Enemy : MonoBehaviour
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

    void Start()
    {
        _logic.Body.OnBodyDeath += OnCharacterDeath;
    }

    #endregion

    void Update()
    {

    }

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
