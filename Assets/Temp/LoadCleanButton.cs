using Settings.SceneManagement;
using UI.Buttons;

public class LoadCleanButton : ButtonBaseUI
{
    public override void ClickListener()
    {
        GameSceneManager.Instance.CleanLoad();
    }
}
