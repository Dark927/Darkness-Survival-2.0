using System.Linq;
using UnityEngine;

namespace UI.SettingsMenu
{
    public class SettingsMenuUI : BasicMenuUI
    {
        [Header("Configuration")]
        private SettingsPanelUI[] _allPanels;
        private SettingsPanelUI _defaultPanel;

        private SettingsPanelUI _currentActivePanel;

        protected override void Awake()
        {
            base.Awake();

            _allPanels = GetComponentsInChildren<SettingsPanelUI>();
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
                OpenTab(_defaultPanel);
            }
        }

        public void OpenTab(SettingsPanelUI panelToOpen)
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
