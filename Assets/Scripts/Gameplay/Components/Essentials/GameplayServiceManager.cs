using Gameplay.Components;
using Gameplay.Visual;
using Settings.Global;
using System;
using UnityEngine;
using Zenject;

public class GameplayServiceManager : SceneServiceManagerBase
{
    #region Fields 

    // ToDo : get this from game settings menu later
    [SerializeField] private bool _useIndicators = false;
    private GamePauseService _pauseService;

    #endregion


    #region Properties

    #endregion


    #region Methods

    #region Init

    [Inject]
    public void Construct(GamePauseService pauseService)
    {
        _pauseService = pauseService;
    }

    private void Awake()
    {
        TryInitIndicators();
        ServiceLocator.Current.Register(_pauseService);
        Services.Add(_pauseService);
    }

    private void TryInitIndicators()
    {
        if (!_useIndicators)
        {
            return;
        }

        GameObject indicatorsServiceObj = new GameObject(nameof(GameplayIndicatorsService));
        indicatorsServiceObj.transform.SetParent(transform, false);
        GameObjectsContainer container = indicatorsServiceObj.AddComponent<GameObjectsContainer>();
        GameplayIndicatorsService indicatorsService = DiContainer.Instantiate<GameplayIndicatorsService>(new object[] { container });

        ServiceLocator.Current.Register(indicatorsService);
        Services.Add(indicatorsService);
    }

    #endregion

    #endregion
}
