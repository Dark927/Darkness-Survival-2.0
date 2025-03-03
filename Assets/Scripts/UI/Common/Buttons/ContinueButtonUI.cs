using Settings.Global;

namespace UI.Buttons
{
    public class ContinueButtonUI : ButtonBaseUI
    {
        public override void ClickListener()
        {
            ServiceLocator.Current.Get<GamePauseService>().UnpauseGame();
            GameplayUI.Instance.DeactivatePauseMenu();
        }
    }
}