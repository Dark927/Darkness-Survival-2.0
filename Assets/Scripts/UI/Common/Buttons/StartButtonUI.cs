using Settings.SceneManagement;

namespace UI.Buttons
{
    public class StartButtonUI : ButtonBaseUI
    {
        public override void ClickListener()
        {
            GameSceneManager.Instance.StartStage();
        }
    }
}
