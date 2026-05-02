using DG.Tweening;
using UnityEngine;

namespace UI.Buttons
{
    public class QuitGameButtonUI : ButtonBaseUI
    {
        public override void Click()
        {
            DOTween.KillAll(false);
            Application.Quit();

#if UNITY_EDITOR
            UnityEditor.EditorApplication.ExitPlaymode();
#endif
        }
    }
}
