using Characters.Common;
using Characters.Common.Abilities;
using Characters.Common.Features;
using Characters.Health;
using UnityEngine;
using Utilities.ErrorHandling;

public class DemonicAbsorptionAbility : MonoBehaviour, IEntityAbility
{
    #region Fields 

    [SerializeField] private IEntityFeature.TargetEntityPart _entityConnectionPart;
    private IAttackableEntityLogic _ownerLogic;
    private IHealth _ownerHealth;

    #endregion


    #region Properties

    public IEntityFeature.TargetEntityPart EntityConnectionPart => _entityConnectionPart;
    public GameObject RootObject => gameObject;

    #endregion


    #region Methods

    #region Init

    public void Initialize(IEntityDynamicLogic characterLogic)
    {
        if (characterLogic is not IAttackableEntityLogic attackableEntityLogic)
        {
            ErrorLogger.Log($"{characterLogic} does not implement {nameof(attackableEntityLogic)}, so this ability can not be used | Deactivating..");
            gameObject.SetActive(false);
            return;
        }

        _ownerLogic = attackableEntityLogic;
        _ownerHealth = _ownerLogic.Body.Health;
        _ownerLogic.OnEnemyKilled += ListenEnemyKilled;
    }

    public void Dispose()
    {
        _ownerLogic.OnEnemyKilled -= ListenEnemyKilled;
    }

    #endregion

    private void ListenEnemyKilled(object sender, IEntityDynamicLogic killedEnemy)
    {
        _ownerHealth.Heal(50f);
    }


    #endregion
}
