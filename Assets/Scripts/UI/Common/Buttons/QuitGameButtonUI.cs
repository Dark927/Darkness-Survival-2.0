using UnityEngine;

namespace UI.Buttons
{
    public class QuitGameButtonUI : ButtonBaseUI
    {
        public override void ClickListener()
        {
            Application.Quit();

#if UNITY_EDITOR
            UnityEditor.EditorApplication.ExitPlaymode();
#endif
        }
    }
}