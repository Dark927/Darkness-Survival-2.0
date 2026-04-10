using UnityEngine;
using System.IO;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Utilities.Json;
using Settings.Global;

public class GraphicsService : IService, IInitializable
{
    private readonly string _savePath = Path.Combine(Application.persistentDataPath, "Settings", "graphics_settings.json");

    private GraphicsSaveData _saveData;
    private List<Resolution> _cachedFilteredResolutions;
    private List<int> _cachedFPSLimits;

    public async UniTask InitializeAsync()
    {
        var (success, result) = await JsonHelper.TryLoadFromJsonAsync<GraphicsSaveData>(_savePath);

        if (success)
        {
            _saveData = result;
        }
        else
        {
            _saveData = new GraphicsSaveData();
        }

        ApplySettingsToEngine();
    }

    public void Initialize()
    {
        InitializeAsync().Forget();
    }

    private void ApplySettingsToEngine()
    {
        QualitySettings.vSyncCount = _saveData.VSyncCount;
        Application.targetFrameRate = _saveData.TargetFrameRate;
        Screen.SetResolution(_saveData.ResolutionWidth, _saveData.ResolutionHeight, _saveData.DisplayMode);
    }

    private void SaveSettings()
    {
        JsonHelper.SaveToJsonAsync(_saveData, _savePath).Forget();
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
        _saveData.VSyncCount = isOn ? 1 : 0;
        QualitySettings.vSyncCount = _saveData.VSyncCount;
        SaveSettings();
    }

    public bool GetCurrentVSync() => _saveData.VSyncCount > 0;

    public void SetFPSLimit(int limit)
    {
        _saveData.TargetFrameRate = limit;
        Application.targetFrameRate = _saveData.TargetFrameRate;
        SaveSettings();
    }

    public int GetCurrentFPSLimit() => _saveData.TargetFrameRate;

    public void SetResolution(Resolution res, FullScreenMode mode)
    {
        _saveData.ResolutionWidth = res.width;
        _saveData.ResolutionHeight = res.height;
        _saveData.DisplayMode = mode;

        Screen.SetResolution(res.width, res.height, mode);
        SaveSettings();
    }

    public FullScreenMode GetCurrentDisplayMode() => _saveData.DisplayMode;

    public Vector2Int GetCurrentResolution()
    {
        // Return the exact values loaded from the JSON save file
        return new Vector2Int(_saveData.ResolutionWidth, _saveData.ResolutionHeight);
    }
}
