
using UnityEngine;

namespace UI.Buttons
{
    public class OpenPanelButtonUI : ButtonBaseUI
    {
        [SerializeField] private GamePanelManager.PanelType _panelType;

        public override void ClickListener()
        {
            GamePanelManager.Instance.OpenPanel(_panelType);
        }
    }
}