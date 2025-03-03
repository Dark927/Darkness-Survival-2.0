using System.Collections;
using System.Collections.Generic;
using UI.Buttons;
using UnityEngine;

namespace UI.Buttons
{
    public class ClosePanelButtonUI : ButtonBaseUI
    {
        public override void ClickListener()
        {
            GamePanelManager.Instance.CloseCurrentPanel();
        }
    }
}
