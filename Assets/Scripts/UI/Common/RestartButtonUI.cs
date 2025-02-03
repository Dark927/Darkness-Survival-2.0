using Settings.SceneManagement;
using UI.Buttons;
using UnityEngine;

public class RestartButtonUI : ButtonBaseUI
{
    public override void ClickListener()
    {
        if (GameSceneManager.Instance != null)
        {
            GameSceneManager.Instance.RestartStage();
        }
        else
        {
            Debug.LogWarning($"Can not Restart the level! - {nameof(GameSceneManager.Instance)} is null!");
        }
    }
}
