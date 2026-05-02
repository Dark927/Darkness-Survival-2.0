using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;
using System;
using System.Collections.Generic;
using Settings.Global;
using Cysharp.Threading.Tasks;
using Characters.Player.Controls;

namespace UI.SettingsMenu
{
    public class ControlsSettingsPanelUI : ScrollablePanelUI, IDisposable
    {
        [Serializable]
        public class RebindUIConfig
        {
            public InputActionReference ActionReference;
            public string CompositeName;
            public Button RebindButton;
            public TMP_Text ButtonText;
            [HideInInspector] public int ResolvedBindingIndex;
        }

        [Header("Rebind Mappings")]
        [Tooltip("Configure your buttons and their corresponding binding indices here.")]
        [SerializeField] private List<RebindUIConfig> _rebindConfigs;

        [Header("UI Feedback")]
        [SerializeField] private string _listeningText = "Press Any Key...";

        private InputService _inputService;

        public override void Initialize()
        {
            base.Initialize();

            _inputService = ServiceLocator.Current.Get<InputService>();

            if (_inputService == null)
            {
                Debug.LogError("InputService not found!");
                return;
            }

            foreach (var config in _rebindConfigs)
            {
                if (config.ActionReference == null) continue;

                // Find the runtime action using the Reference's GUID against the live instance
                InputAction runtimeAction = _inputService.InputActions.FindAction(config.ActionReference.action.id.ToString());
                if (runtimeAction == null) continue;

                config.ResolvedBindingIndex = 0;

                if (!string.IsNullOrEmpty(config.CompositeName))
                {
                    for (int i = 0; i < runtimeAction.bindings.Count; i++)
                    {
                        if (runtimeAction.bindings[i].name.Equals(config.CompositeName, StringComparison.OrdinalIgnoreCase))
                        {
                            config.ResolvedBindingIndex = i;
                            break;
                        }
                    }
                }

                config.ButtonText.text = _inputService.GetBindingName(runtimeAction, config.ResolvedBindingIndex);

                config.RebindButton.onClick.RemoveAllListeners();
                config.RebindButton.onClick.AddListener(() => OnRebindButtonClicked(config, runtimeAction));
            }
        }

        private void OnRebindButtonClicked(RebindUIConfig config, InputAction action)
        {
            string previousText = config.ButtonText.text;
            Color previousTextColor = config.ButtonText.color;
            config.ButtonText.text = _listeningText;

            SetButtonsInteractable(false);

            _inputService.StartRebind(
                actionToRebind: action,
                bindingIndex: config.ResolvedBindingIndex,
                onComplete: (newKeyName) =>
                {
                    config.ButtonText.text = newKeyName;
                    SetButtonsInteractable(true);
                },
                onCancel: () =>
                {
                    config.ButtonText.text = previousText;
                    SetButtonsInteractable(true);
                },
                // Handle the error dynamically
                onError: async (errorMessage) =>
                {
                    // Show the error in red
                    config.ButtonText.text = errorMessage;
                    config.ButtonText.color = Color.red;

                    // Wait for 1.5 seconds without freezing the game
                    await UniTask.Delay(1500, true);

                    // Revert the UI back to how it was before they clicked
                    config.ButtonText.text = previousText;
                    config.ButtonText.color = previousTextColor;
                    SetButtonsInteractable(true);
                }
            );
        }

        private void SetButtonsInteractable(bool state)
        {
            foreach (var config in _rebindConfigs)
            {
                if (config.RebindButton != null)
                {
                    config.RebindButton.interactable = state;
                }
            }
        }

        private void OnDestroy()
        {
            Dispose();
        }

        public void Dispose()
        {
            foreach (var config in _rebindConfigs)
            {
                if (config.RebindButton != null)
                {
                    config.RebindButton.onClick.RemoveAllListeners();
                }
            }
        }
    }
}
