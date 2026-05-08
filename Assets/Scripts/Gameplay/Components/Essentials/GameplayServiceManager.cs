using Gameplay.Visual;
using Settings.Global;
using UnityEngine;
using Zenject;

namespace Gameplay.Components
{
    public class GameplayServiceManager : SceneServiceManagerBase
    {
        #region Fields 

        // ToDo : get this from game settings menu later
        [SerializeField] private bool _useIndicators = false;

        #endregion


        #region Properties

        #endregion


        #region Methods

        #region Init


        private void Awake()
        {
            TryInitIndicators();
            TryInitGameplayObjectContainers();
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

        private void TryInitGameplayObjectContainers()
        {
            // Create the root GameObject in the scene hierarchy
            GameObject containersServiceObj = new GameObject(nameof(GameplayContainersService));
            containersServiceObj.transform.SetParent(transform, false);

            GameObjectsContainer rootContainer = containersServiceObj.AddComponent<GameObjectsContainer>();

            GameplayContainersService containersService = DiContainer.Instantiate<GameplayContainersService>(new object[] { rootContainer });

            ServiceLocator.Current.Register(containersService);
            Services.Add(containersService);
        }

        #endregion

        #endregion
    }
}
