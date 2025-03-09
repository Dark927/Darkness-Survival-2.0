using Settings.SceneManagement;


namespace UI.Buttons
{
    public class ExitMenuButtonUI : ButtonBaseUI
    {
        public override void Click()
        {
            GameSceneManager.Instance.LoadMainMenu();
        }
    }
}
