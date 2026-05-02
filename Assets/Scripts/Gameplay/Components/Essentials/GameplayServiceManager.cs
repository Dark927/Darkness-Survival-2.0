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
}
