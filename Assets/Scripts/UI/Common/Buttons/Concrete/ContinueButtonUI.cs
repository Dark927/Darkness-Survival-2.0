using System.Collections;
using Settings.Global;
using UnityEngine;

namespace UI.Buttons
{
    public class ContinueButtonUI : ButtonBaseUI
    {
        public override void Click()
        {
            GamePauseService pauseService = ServiceLocator.Current.Get<GamePauseService>();
            GameplayUI.Instance.DeactivatePauseMenu(() => pauseService.UnpauseGame());
        }
    }
}
