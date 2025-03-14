using Settings.Global;
using Settings.SceneManagement;
using UnityEngine;
using Zenject;


namespace UI.Buttons
{
    public class ExitMenuButtonUI : ButtonBaseUI
    {
        public override void Click()
        {
            GameStateService stateService = ServiceLocator.Current.Get<GameStateService>();
            stateService.ExitToMenu();
        }
    }
}
