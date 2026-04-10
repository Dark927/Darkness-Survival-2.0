using System;
using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;
using Cysharp.Threading.Tasks;
using Utilities.Json;
using Settings.Global;
using Characters.Player.Controls;

public class InputService : IService, IInitializable
{
    private readonly string _savePath = Path.Combine(Application.persistentDataPath, "Settings", "input_settings.json");
    private InputSaveData _saveData;
    private InputActionRebindingExtensions.RebindingOperation _rebindOperation;

    // The single source of truth for game's inputs
    public PlayerInputActions InputActions { get; private set; }


    public InputService()
    {
        InputActions = new PlayerInputActions();
        // CRUCIAL: Enable the actions so they can be listened to while in the UI!
        InputActions.Enable();
    }

    public async UniTask InitializeAsync()
    {
        var (success, result) = await JsonHelper.TryLoadFromJsonAsync<InputSaveData>(_savePath);
        _saveData = success ? result : new InputSaveData();

        // Apply saved overrides if they exist
        if (!string.IsNullOrEmpty(_saveData.BindingOverrides))
        {
            InputActions.LoadBindingOverridesFromJson(_saveData.BindingOverrides);
        }
    }

    public void Initialize()
    {
        InitializeAsync().Forget();
    }

    private void SaveSettings()
    {
        _saveData.BindingOverrides = InputActions.SaveBindingOverridesAsJson();
        JsonHelper.SaveToJsonAsync(_saveData, _savePath).Forget();
    }

    /// <summary>
    /// Starts the interactive rebinding process for a specific action and binding index.
    /// </summary>
    public void StartRebind(InputAction actionToRebind, int bindingIndex, Action<string> onComplete, Action onCancel)
    {
        // Actions must be disabled before rebinding
        actionToRebind.Disable();

        // Clean up any existing operation
        _rebindOperation?.Cancel();
        _rebindOperation?.Dispose();

        _rebindOperation = actionToRebind.PerformInteractiveRebinding(bindingIndex)
            // Exclude mouse movement, otherwise moving the mouse instantly binds it
            .WithControlsExcluding("Mouse/position")
            .WithControlsExcluding("Mouse/delta")
            // Pressing Escape cancels the rebind
            .WithCancelingThrough("<Keyboard>/escape")
            .OnComplete(operation =>
            {
                operation.Dispose();
                actionToRebind.Enable();
                SaveSettings();

                // Return the new readable key name (e.g., "W", "LMB")
                string newKey = actionToRebind.GetBindingDisplayString(bindingIndex);
                onComplete?.Invoke(newKey);
            })
            .OnCancel(operation =>
            {
                operation.Dispose();
                actionToRebind.Enable();
                onCancel?.Invoke();
            })
            .Start(); // Begin listening for input
    }

    public string GetBindingName(InputAction action, int bindingIndex)
    {
        return action.GetBindingDisplayString(bindingIndex);
    }
}
