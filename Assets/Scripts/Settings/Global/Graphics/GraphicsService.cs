using UnityEngine;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Settings.Global;

public class GraphicsService : IService, IInitializable
{
    private List<Resolution> _cachedFilteredResolutions;
    private List<int> _cachedFPSLimits;

    private ISettingsStorage _settingsStorage;
    public GraphicsSaveData SaveData => _settingsStorage.Data.Graphics;


    public GraphicsService(ISettingsStorage settingsStorage)
    {
        _settingsStorage = settingsStorage;
    }

    public async UniTask InitializeAsync()
    {
        await UniTask.WaitUntil(() => _settingsStorage.IsLoaded);
        ApplySettingsToEngine();
    }

    public void Initialize()
    {
        InitializeAsync().Forget();
    }

    private void ApplySettingsToEngine()
    {
        QualitySettings.vSyncCount = SaveData.VSyncCount;
        Application.targetFrameRate = SaveData.TargetFrameRate;
        Screen.SetResolution(SaveData.ResolutionWidth, SaveData.ResolutionHeight, SaveData.DisplayMode);
    }

    private void SaveSettings()
    {
        _settingsStorage.SaveAllSettings();
    }


    // --- Hardware Data Providers ---

    public List<Resolution> GetFilteredResolutions()
    {
        // Cache the list so we don't recalculate dictionary allocations every time the UI opens
        if (_cachedFilteredResolutions != null) return _cachedFilteredResolutions;

        _cachedFilteredResolutions = new List<Resolution>();
        Dictionary<string, Resolution> uniqueResolutions = new Dictionary<string, Resolution>();

        foreach (Resolution res in Screen.resolutions)
        {
            string formatString = $"{res.width} x {res.height}";

            // Store the resolution if it's new, OR overwrite it if the new one has a higher refresh rate
            if (!uniqueResolutions.ContainsKey(formatString) || res.refreshRateRatio.value > uniqueResolutions[formatString].refreshRateRatio.value)
            {
                uniqueResolutions[formatString] = res;
            }
        }

        _cachedFilteredResolutions.AddRange(uniqueResolutions.Values);
        return _cachedFilteredResolutions;
    }

    public List<int> GetAvailableFPSLimits()
    {
        if (_cachedFPSLimits != null) return _cachedFPSLimits;

        _cachedFPSLimits = new List<int>();
        int maxMonitorHz = Mathf.RoundToInt((float)Screen.currentResolution.refreshRateRatio.value);
        int[] standardLimits = { 30, 60, 90, 120, 144, 165, 240, 360 };

        foreach (int limit in standardLimits)
        {
            if (limit <= maxMonitorHz)
            {
                _cachedFPSLimits.Add(limit);
            }
        }

        if (!_cachedFPSLimits.Contains(maxMonitorHz))
        {
            _cachedFPSLimits.Add(maxMonitorHz);
            _cachedFPSLimits.Sort();
        }

        _cachedFPSLimits.Add(-1);
        return _cachedFPSLimits;
    }


    // --- Public API for the UI ---

    public void SetVSync(bool isOn)
    {
        SaveData.VSyncCount = isOn ? 1 : 0;
        QualitySettings.vSyncCount = SaveData.VSyncCount;
        SaveSettings();
    }

    public bool GetCurrentVSync() => SaveData.VSyncCount > 0;

    public void SetFPSLimit(int limit)
    {
        SaveData.TargetFrameRate = limit;
        Application.targetFrameRate = SaveData.TargetFrameRate;
        SaveSettings();
    }

    public int GetCurrentFPSLimit() => SaveData.TargetFrameRate;

    public void SetResolution(Resolution res, FullScreenMode mode)
    {
        SaveData.ResolutionWidth = res.width;
        SaveData.ResolutionHeight = res.height;
        SaveData.DisplayMode = mode;

        Screen.SetResolution(res.width, res.height, mode);
        SaveSettings();
    }

    public FullScreenMode GetCurrentDisplayMode() => SaveData.DisplayMode;

    public Vector2Int GetCurrentResolution()
    {
        // Return the exact values loaded from the Storage
        return new Vector2Int(SaveData.ResolutionWidth, SaveData.ResolutionHeight);
    }
}
