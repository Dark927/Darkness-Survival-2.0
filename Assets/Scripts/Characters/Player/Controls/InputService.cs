using System;
using UnityEngine.InputSystem;
using Cysharp.Threading.Tasks;
using Settings.Global;

namespace Characters.Player.Controls
{
    public class InputService : IService, IInitializable
    {
        public PlayerInputActions InputActions { get; private set; }

        private InputActionRebindingExtensions.RebindingOperation _rebindOperation;
        private ISettingsStorage _settingsStorage;
        public InputSaveData SaveData => _settingsStorage.Data.Input;


        public InputService(ISettingsStorage settingsStorage)
        {
            InputActions = new PlayerInputActions();
            InputActions.Enable();
            _settingsStorage = settingsStorage;
        }

        public async UniTask InitializeAsync()
        {
            await UniTask.WaitUntil(() => _settingsStorage.IsLoaded);

            if (!string.IsNullOrEmpty(SaveData.BindingOverrides))
            {
                InputActions.LoadBindingOverridesFromJson(SaveData.BindingOverrides);
            }
        }

        public void Initialize()
        {
            InitializeAsync().Forget();
        }

        private void SaveSettings()
        {
            SaveData.BindingOverrides = InputActions.SaveBindingOverridesAsJson();
            _settingsStorage.SaveAllSettings();
        }

        public void StartRebind(InputAction actionToRebind, int bindingIndex, Action<string> onComplete, Action onCancel, Action<string> onError)
        {
            actionToRebind.Disable();

            // We must remember what the override was before the user messed with it
            string previousOverridePath = actionToRebind.bindings[bindingIndex].overridePath;

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

                    if (IsDuplicateBinding(newPath, actionToRebind, bindingIndex))
                    {
                        // RESTORE THE EXACT PREVIOUS STATE
                        if (string.IsNullOrEmpty(previousOverridePath))
                        {
                            actionToRebind.RemoveBindingOverride(bindingIndex);
                        }
                        else
                        {
                            actionToRebind.ApplyBindingOverride(bindingIndex, previousOverridePath);
                        }

                        actionToRebind.Enable();

                        // Fire the error callback to the UI
                        onError?.Invoke("USED!");
                        return;
                    }

                    actionToRebind.Enable();

                    // Only save settings if we successfully kept the new key
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
}
