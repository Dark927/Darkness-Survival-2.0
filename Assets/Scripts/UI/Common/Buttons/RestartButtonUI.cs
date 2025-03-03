using Settings.SceneManagement;
using UI.Buttons;
using UnityEngine;

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