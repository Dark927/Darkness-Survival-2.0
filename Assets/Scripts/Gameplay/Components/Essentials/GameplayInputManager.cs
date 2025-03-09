using UnityEngine;
using UnityEngine.InputSystem;

namespace Gameplay.Components
{
    [RequireComponent(typeof(GameStateManager))]
    public class GameplayInputManager : MonoBehaviour
    {
        #region Fields 

        private GameStateManager _gameStateManager;

        #endregion


        #region Properties

        #endregion


        #region Methods

        #region Init

        private void Awake()
        {
            _gameStateManager = GetComponent<GameStateManager>();
        }

        #endregion


        public void PauseKeyListener(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                _gameStateManager.SwitchPauseState();
            }
        }

        #endregion
    }
}
