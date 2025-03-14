using System;
using UnityEngine;
using Zenject;

namespace UI
{
    public class BasicMenuUI : MonoBehaviour, IMenuUI
    {
        private PopupUI _panelPopup;
        private GamePanelManagerUI _panelManagerUI;

        public GamePanelManagerUI OwnerPanelManager => _panelManagerUI;


        [Inject]
        public void Construct(GamePanelManagerUI panelManagerUI)
        {
            _panelManagerUI = panelManagerUI;
        }

        protected virtual void Awake()
        {
            _panelPopup = GetComponentInChildren<PopupUI>();
        }

        public virtual void Activate(Action callback = null)
        {
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

        public void RequestDeactivation(Action callerCallback = null, float speedMult = 1f)
        {
            if (OwnerPanelManager.CanClosePanel(this, out Action ownerCloseCallback))
            {
                Deactivate(callerCallback, ownerCloseCallback, speedMult);
            }
        }

        protected virtual void Deactivate(Action callerCallback = null, Action ownerCloseCallback = null, float speedMult = 1f)
        {
            if (_panelPopup != null)
            {
                _panelPopup.Hide(() =>
                {
                    ownerCloseCallback?.Invoke();
                    callerCallback?.Invoke();
                });
            }
            else
            {
                ownerCloseCallback?.Invoke();
                callerCallback?.Invoke();
            }
        }
    }
}
