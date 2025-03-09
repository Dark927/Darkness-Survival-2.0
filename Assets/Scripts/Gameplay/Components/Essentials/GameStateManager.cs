using Settings.Global;
using UI;
using UnityEngine;
using Zenject;

namespace Gameplay.Components
{
    public class GameStateManager : MonoBehaviour
    {
        #region Fields 

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

        #endregion

        public void PauseGame()
        {
            _pauseService.PauseGame();
            GameplayUI.Instance.ActivatePauseMenu();
        }

        public void UnpauseGame()
        {
            GameplayUI.Instance.DeactivatePauseMenu(() => _pauseService.UnpauseGame());
        }

        public void SwitchPauseState()
        {
            if (!_pauseService.IsGamePaused)
            {
                PauseGame();
            }
            else
            {
                UnpauseGame();
            }
        }

        private void OnDestroy()
        {
            GameplayUI.Instance.DeactivatePauseMenu(() => _pauseService.UnpauseGame());
        }

        #endregion
    }
}
