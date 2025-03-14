using Settings.SceneManagement;
using UI.Buttons;

public class UnloadButton : ButtonBaseUI
{
    public override void Click()
    {
        GameSceneLoadHandler.Instance.SceneLoader.UnloadAll();
    }
}
