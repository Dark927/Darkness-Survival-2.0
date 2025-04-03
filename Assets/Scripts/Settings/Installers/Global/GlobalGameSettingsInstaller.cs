using System.Collections.Generic;
using Settings.Global.Audio;
using UI;
using UnityEngine;
using Utilities.Attributes;
using Zenject;

namespace Settings.Global
{
    public class GlobalGameSettingsInstaller : MonoInstaller
    {
        [CustomHeader("Game Panel Manager UI - Settings", count = 2, depth = 0)]
        [SerializeField] private Canvas _targetCanvas;
        [SerializeField] private GamePanelDataUI _panelData;

        [CustomHeader("Core Settings", count = 1, depth = 0)]
        [SerializeField] private Transform _settingsComponentsContainer;

        [CustomHeader("Audio Settings", count = 1, depth = 1, headerColor = CustomHeaderAttribute.HeaderColor.cyan)]
        [SerializeField] private AudioProviderData _audioProviderData;

        #region Methods

        public override void InstallBindings()
        {
            BindPanelManager();
            BindAudioProvider();
        }

        private void BindAudioProvider()
        {
            GameObject audioProviderObject = new GameObject(nameof(AudioProviderMono), typeof(AudioProviderMono));
            audioProviderObject.transform.SetParent(_settingsComponentsContainer, false);
            AudioProviderMono audioProvider = audioProviderObject.GetComponent<AudioProviderMono>();

            audioProvider.Initialize(_audioProviderData.Settings);

            Container
                .Bind<IAudioProvider>()
                .To<AudioProviderMono>()
                .FromInstance(audioProvider)
                .AsSingle()
                .NonLazy();
        }

        private void BindPanelManager()
        {
            GamePanelManagerUI panelManager = FindAnyObjectByType<GamePanelManagerUI>();

            if (panelManager == null)
            {
                GameObject panelManagerObj = new GameObject("GamePanelManagerUI", new System.Type[] { typeof(GamePanelManagerUI) });
                panelManager = panelManagerObj.GetComponent<GamePanelManagerUI>();
            }

            panelManager.Initialize(Container, _targetCanvas, _panelData);

            Container
                .Bind<GamePanelManagerUI>()
                .FromInstance(panelManager)
                .AsSingle()
                .NonLazy();
        }

        #endregion
    }
}
