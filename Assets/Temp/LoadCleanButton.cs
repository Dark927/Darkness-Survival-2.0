using Settings.SceneManagement;
using UI.Buttons;

public class LoadCleanButton : ButtonBaseUI
{
    public override void Click()
    {
        GameSceneManager.Instance.CleanLoad();
    }
}
