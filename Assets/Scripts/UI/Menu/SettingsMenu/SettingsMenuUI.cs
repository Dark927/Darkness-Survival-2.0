using System.Linq;
using UnityEngine;

namespace UI.SettingsMenu
{
    public class SettingsMenuUI : BasicMenuUI
    {
        [Header("Configuration")]
        private PanelUI[] _allPanels;
        private PanelUI _defaultPanel;

        private PanelUI _currentActivePanel;

        protected override void Awake()
        {
            base.Awake();

            _allPanels = GetComponentsInChildren<PanelUI>();
            _defaultPanel = _allPanels.FirstOrDefault();

            // Initialize all panels and hide them instantly
            foreach (var panel in _allPanels)
            {
                panel.Initialize();
                panel.Hide(instant: true);
            }
        }

        private void Start()
        {
            // Open the default panel on start
            if (_defaultPanel != null)
            {
                transform.SetAsLastSibling();
                OpenTab(_defaultPanel);
            }
        }

        public void OpenTab(PanelUI panelToOpen)
        {
            // Ignore if we are already on this tab
            if (panelToOpen == _currentActivePanel)
            {
                return;
            }

            // Hide the current tab
            if (_currentActivePanel != null)
            {
                _currentActivePanel.Hide();
            }

            // Set the new tab and show it
            _currentActivePanel = panelToOpen;
            _currentActivePanel.Show();
        }
    }
}
