using Zenject;
using UnityEngine;
using Settings.Global;
using Settings;
using Gameplay.Visual;

public class GameplayEssentialsInstaller : MonoInstaller
{
    [Header("Main parameters")]

    [Header("Indicator Service - Settings")]
    [SerializeField] private IndicatorPoolData _indicatorPoolData;
    [SerializeField] private IndicatorServiceData _indicatorServiceData;

    public override void InstallBindings()
    {
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
