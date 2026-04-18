using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using Settings.Global;

namespace UI.SettingsMenu
{
    public class GraphicsSettingsPanelUI : SettingsPanelUI, IDisposable
    {
        [Header("UI Controls")]
        [SerializeField] private TMP_Dropdown _resolutionDropdown;
        [SerializeField] private TMP_Dropdown _displayModeDropdown;
        [SerializeField] private Toggle _vSyncToggle;
        [SerializeField] private TMP_Dropdown _fpsDropdown;

        private GraphicsService _graphicsService;
        private List<Resolution> _resolutions;
        private List<int> _fpsLimits;


        #region Init 

        public override void Initialize()
        {
            base.Initialize();

            _graphicsService = ServiceLocator.Current.Get<GraphicsService>();

            if (_graphicsService == null)
            {
                Debug.LogError("GraphicsService not found in Service Locator!");
                return;
            }

            // --- Fetch Cached Data from Service ---
            _resolutions = _graphicsService.GetFilteredResolutions();
            _fpsLimits = _graphicsService.GetAvailableFPSLimits();

            // --- Setup UI ---
            _vSyncToggle.isOn = _graphicsService.GetCurrentVSync();
            _vSyncToggle.onValueChanged.RemoveAllListeners();
            _vSyncToggle.onValueChanged.AddListener(OnVSyncToggled);

            SetupDisplayModeDropdown();
            SetupResolutionDropdown();
            SetupFPSDropdown();
        }

        private void SetupDisplayModeDropdown()
        {
            _displayModeDropdown.onValueChanged.RemoveAllListeners();
            _displayModeDropdown.ClearOptions();
            _displayModeDropdown.AddOptions(new List<string> { "Fullscreen", "Fullscreen Window", "Windowed" });

            FullScreenMode currentMode = _graphicsService.GetCurrentDisplayMode();

            _displayModeDropdown.value = currentMode switch
            {
                FullScreenMode.ExclusiveFullScreen => 0,
                FullScreenMode.FullScreenWindow => 1,
                FullScreenMode.Windowed => 2,
                _ => 0
            };

            _displayModeDropdown.RefreshShownValue();
            _displayModeDropdown.onValueChanged.AddListener(OnDisplayModeChanged);
        }

        private void SetupResolutionDropdown()
        {
            _resolutionDropdown.onValueChanged.RemoveAllListeners();

            _resolutionDropdown.ClearOptions();

            List<string> options = new List<string>();
            int currentResolutionIndex = 0;

            Vector2Int savedRes = _graphicsService.GetCurrentResolution();

            for (int i = 0; i < _resolutions.Count; i++)
            {
                options.Add($"{_resolutions[i].width} x {_resolutions[i].height}");

                if (_resolutions[i].width == savedRes.x && _resolutions[i].height == savedRes.y)
                {
                    currentResolutionIndex = i;
                }
            }

            _resolutionDropdown.AddOptions(options);
            _resolutionDropdown.value = currentResolutionIndex;
            _resolutionDropdown.RefreshShownValue();

            _resolutionDropdown.onValueChanged.AddListener(OnResolutionChanged);
        }

        private void SetupFPSDropdown()
        {
            _fpsDropdown.onValueChanged.RemoveAllListeners();
            _fpsDropdown.ClearOptions();
            List<string> options = new List<string>();

            foreach (int limit in _fpsLimits)
            {
                options.Add(limit == -1 ? "Unlimited" : $"{limit} FPS");
            }

            _fpsDropdown.AddOptions(options);

            int currentFPS = _graphicsService.GetCurrentFPSLimit();
            int currentIndex = _fpsLimits.IndexOf(currentFPS);

            if (currentIndex == -1) currentIndex = _fpsLimits.Count - 1;

            _fpsDropdown.value = currentIndex;
            _fpsDropdown.RefreshShownValue();

            _fpsDropdown.onValueChanged.AddListener(OnFPSChanged);
        }

        #endregion

        #region Cleanup Phase

        private void OnDestroy()
        {
            Dispose();
        }

        public void Dispose()
        {
            if (_vSyncToggle != null) _vSyncToggle.onValueChanged.RemoveListener(OnVSyncToggled);
            if (_displayModeDropdown != null) _displayModeDropdown.onValueChanged.RemoveListener(OnDisplayModeChanged);
            if (_resolutionDropdown != null) _resolutionDropdown.onValueChanged.RemoveListener(OnResolutionChanged);
            if (_fpsDropdown != null) _fpsDropdown.onValueChanged.RemoveListener(OnFPSChanged);
        }

        #endregion

        #region UI Event Listeners

        private void OnVSyncToggled(bool isOn)
        {
            _graphicsService.SetVSync(isOn);
        }

        private void OnDisplayModeChanged(int index)
        {
            FullScreenMode mode = FullScreenMode.ExclusiveFullScreen;
            if (index == 1) mode = FullScreenMode.FullScreenWindow;
            if (index == 2) mode = FullScreenMode.Windowed;

            _graphicsService.SetResolution(_resolutions[_resolutionDropdown.value], mode);
        }

        private void OnResolutionChanged(int index)
        {
            Resolution res = _resolutions[index];
            FullScreenMode mode = _graphicsService.GetCurrentDisplayMode();
            _graphicsService.SetResolution(res, mode);
        }

        private void OnFPSChanged(int index)
        {
            _graphicsService.SetFPSLimit(_fpsLimits[index]);
        }

        #endregion
    }
}
