using UnityEngine;
using UnityEngine.UI;

namespace UI.SettingsMenu
{
    [RequireComponent(typeof(ScrollRect))]
    public class ScrollablePanelUI : PanelUI
    {
        private ScrollJumpFixerUI _scrollFixer;

        public override void Initialize()
        {
            base.Initialize();
            _scrollFixer = GetComponent<ScrollJumpFixerUI>();
        }

        public override void Show()
        {
            base.Show();

            if (_scrollFixer != null)
            {
                _scrollFixer.SnapToTop();
            }
        }
    }
}
