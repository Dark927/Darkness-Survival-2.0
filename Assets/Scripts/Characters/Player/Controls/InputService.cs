using System;
using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;
using Cysharp.Threading.Tasks;
using Utilities.Json;
using Settings.Global;

public class InputService : IService, IInitializable
{
    private readonly string _savePath = Path.Combine(Application.persistentDataPath, "Settings", "input_settings.json");

    public PlayerInputActions InputActions { get; private set; }

    private InputSaveData _saveData;
    private InputActionRebindingExtensions.RebindingOperation _rebindOperation;

    public InputService()
    {
        InputActions = new PlayerInputActions();
        InputActions.Enable();
    }

    public async UniTask InitializeAsync()
    {
        var (success, result) = await JsonHelper.TryLoadFromJsonAsync<InputSaveData>(_savePath);
        _saveData = success ? result : new InputSaveData();

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

    public void StartRebind(InputAction actionToRebind, int bindingIndex, Action<string> onComplete, Action onCancel, Action<string> onError)
    {
        actionToRebind.Disable();

        _rebindOperation?.Cancel();
        _rebindOperation?.Dispose();

        _rebindOperation = actionToRebind.PerformInteractiveRebinding(bindingIndex)
            .WithControlsExcluding("Mouse/position")
            .WithControlsExcluding("Mouse/delta")
            .WithCancelingThrough("<Keyboard>/escape")
            .OnComplete(operation =>
            {
                operation.Dispose();

                string newPath = actionToRebind.bindings[bindingIndex].effectivePath;

                // Check if another action is already using this path
                if (IsDuplicateBinding(newPath, actionToRebind, bindingIndex))
                {
                    // Reject the override
                    actionToRebind.RemoveBindingOverride(bindingIndex);
                    actionToRebind.Enable();

                    // Fire the error callback to the UI
                    onError?.Invoke("USED!");
                    return;
                }

                actionToRebind.Enable();
                SaveSettings();

                string newKey = actionToRebind.GetBindingDisplayString(bindingIndex);
                onComplete?.Invoke(newKey);
            })
            .OnCancel(operation =>
            {
                operation.Dispose();
                actionToRebind.Enable();
                onCancel?.Invoke();
            })
            .Start();
    }

    public string GetBindingName(InputAction action, int bindingIndex)
    {
        return action.GetBindingDisplayString(bindingIndex);
    }

    // Duplication Checker
    private bool IsDuplicateBinding(string newPath, InputAction actionToRebind, int bindingIndex)
    {
        // Loop through every action in your entire Input Map
        foreach (InputAction action in InputActions)
        {
            for (int i = 0; i < action.bindings.Count; i++)
            {
                if (action == actionToRebind && i == bindingIndex) continue;

                if (action.bindings[i].effectivePath == newPath)
                {
                    return true;
                }
            }
        }
        return false;
    }
}
