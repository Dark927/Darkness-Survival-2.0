using System;

namespace UI
{
    public class PauseMenuUI : MenuWithVideoUI
    {
        private PopupUI _panelPopup;

        protected override void Awake()
        {
            base.Awake();
            _panelPopup = GetComponent<PopupUI>();
        }

        public override void Activate(Action callback = null)
        {
            ActivateVideoBackground();

            if (_panelPopup != null)
            {
                _panelPopup.PrepareAnimation();
                _panelPopup.Show(callback);
            }
            else
            {
                callback?.Invoke();
            }
        }

        public override void Deactivate(Action callback = null, float speedMult = 1f)
        {
            DeactivateVideoBackground(speedMult);

            if (_panelPopup != null)
            {
                _panelPopup.Hide(callback);
            }
            else
            {
                callback?.Invoke();
            }
        }
    }
}
