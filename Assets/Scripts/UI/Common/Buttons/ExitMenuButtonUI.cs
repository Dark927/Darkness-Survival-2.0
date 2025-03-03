using Settings.Global;
using Settings.SceneManagement;


namespace UI.Buttons
{
    public class ExitMenuButtonUI : ButtonBaseUI
    {
        public override void ClickListener()
        {
            GameSceneManager.Instance.LoadMainMenu();
        }
    }
}