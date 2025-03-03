using Settings.SceneManagement;

namespace UI.Buttons
{
    public class RestartButtonUI : ButtonBaseUI
    {
        public override void ClickListener()
        {
            GameSceneManager.Instance.StartStage();
        }
    }
}