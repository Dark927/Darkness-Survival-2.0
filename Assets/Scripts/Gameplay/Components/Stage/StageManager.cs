using Gameplay.Components;
using Gameplay.Components.Enemy;
using Settings.Global;
using Zenject;

public class StageManager : SceneServiceManagerBase
{
    #region Fields 

    #endregion


    #region Properties

    #endregion


    #region Methods

    #region Init

    private void Awake()
    {
        // ToDo : remove this later, use another logic to Create and Init this component!
        CreateEnemyManagementService();
    }

    private void Start()
    {
        // ToDo : remove this later, use another logic to Create and Init this component!
        foreach (var service in Services)
        {
            if (service is Settings.Global.IInitializable initService)
            {
                initService.Initialize();
            }
        }
    }

    private void CreateEnemyManagementService()
    {
        EnemyManagementService enemyManagementService = DiContainer.Instantiate<EnemyManagementService>();
        ServiceLocator.Current.Register(enemyManagementService);
        Services.Add(enemyManagementService);
    }


    #endregion

    #endregion
}
