using Settings.SceneManagement;
using UI.Buttons;

public class UnloadButton : ButtonBaseUI
{
    public override void ClickListener()
    {
        GameSceneManager.Instance.UnloadAll();
    }
}
