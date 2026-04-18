using UnityEngine;
using DG.Tweening;

namespace UI.SettingsMenu
{
    [RequireComponent(typeof(CanvasGroup))]
    public class SettingsPanelUI : MonoBehaviour
    {
        protected CanvasGroup canvasGroup;

        public virtual void Initialize()
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }

        public virtual void Show()
        {
            canvasGroup.DOKill();

            canvasGroup.DOFade(1f, 0.2f);

            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
        }

        public virtual void Hide(bool instant = false)
        {
            canvasGroup.DOKill();

            if (instant)
            {
                canvasGroup.alpha = 0f;
            }
            else
            {
                canvasGroup.DOFade(0f, 0.2f);
            }

            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }
    }
}
