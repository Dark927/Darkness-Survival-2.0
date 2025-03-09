using Settings.SceneManagement;

namespace UI.Buttons
{
    public class RestartButtonUI : ButtonBaseUI
    {
        public override void Click()
        {
            GameSceneManager.Instance.StartStage();
        }
    }
}
