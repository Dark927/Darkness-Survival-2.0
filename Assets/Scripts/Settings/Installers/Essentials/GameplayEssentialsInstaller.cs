using Gameplay.Components;
using Gameplay.Visual;
using Settings;
using Settings.Global;
using UI;
using UnityEngine;
using Zenject;

namespace Settings.Installers
{
    public class GameplayEssentialsInstaller : MonoInstaller
    {
        [Header("Main parameters")]

        [Header("Indicator Service - Settings")]
        [SerializeField] private IndicatorPoolData _indicatorPoolData;
        [SerializeField] private IndicatorServiceData _indicatorServiceData;

        public override void InstallBindings()
        {
            GamePanelManagerUI panelManager = FindAnyObjectByType<GamePanelManagerUI>();

            Container
                .Bind<GamePanelManagerUI>()
                .FromInstance(panelManager)
                .AsSingle();

            Container
                .Bind<IndicatorPoolData>()
                .FromInstance(_indicatorPoolData)
                .AsSingle();

            Container
                .Bind<IndicatorServiceData>()
                .FromInstance(_indicatorServiceData)
                .AsSingle();
        }

        private void TryBindDiContainer()
        {
            if (!Container.HasBinding<DiContainer>())
            {
                Container
                    .Bind<DiContainer>()
                    .FromInstance(Container)
                    .AsSingle();
            }
        }

    }
}
