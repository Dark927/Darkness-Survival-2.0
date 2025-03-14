
using UnityEngine;
using Zenject;

namespace UI.Buttons
{
    public class OpenPanelButtonUI : ButtonBaseUI
    {
        [SerializeField] private GamePanelManagerUI.PanelType _panelType;

        public override void Click()
        {
            ParentMenu.OwnerPanelManager.TryOpenPanel(_panelType);
        }
    }
}
