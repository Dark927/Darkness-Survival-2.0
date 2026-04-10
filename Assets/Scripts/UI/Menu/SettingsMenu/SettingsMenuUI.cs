using System.Collections.Generic;
using UI;
using UnityEngine;

public class SettingsMenuUI : BasicMenuUI
{
    [Header("Configuration")]
    [Tooltip("Drag all panels (Graphics, Audio, Controls) here.")]
    [SerializeField] private List<SettingsPanelUI> _allPanels;

    [Tooltip("The panel that should be visible when the menu opens.")]
    [SerializeField] private SettingsPanelUI _defaultPanel;

    private SettingsPanelUI _currentActivePanel;

    protected override void Awake()
    {
        base.Awake();

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
