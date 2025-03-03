using System;
using Settings;
using UnityEngine;
using Utilities.ErrorHandling;
using Zenject;

[CreateAssetMenu(fileName = "NewGameplayStageConfigInstaller", menuName = "Installers/Gameplay Stage Config Installer")]
public class GameplayStageConfigInstaller : ScriptableObjectInstaller<GameplayStageConfigInstaller>
{
    [SerializeField] private GlobalGameplayStageConfig _gameplayConfig;

    public override void InstallBindings()
    {
        try
        {
            if (_gameplayConfig == null)
            {
                throw new NullReferenceException(nameof(GlobalGameplayStageConfig));
            }

            BindSettings();
            TryBindDiContainer();
        }
        catch (Exception e)
        {
            ErrorLogger.LogComponentIsNull(LogOutputType.Console, name, e.Message);
        }
    }

    private void BindSettings()
    {
        BindContainers();
        BindEnemySettings();
    }

    private void BindContainers()
    {
        Container
            .Bind<EnemyPoolData>()
            .FromScriptableObject(_gameplayConfig.EnemyPoolsData)
            .AsSingle();

        Container
            .Bind<ItemPoolData>()
            .FromScriptableObject(_gameplayConfig.ItemPoolsData)
            .AsSingle();
    }

    private void BindEnemySettings()
    {
        Container
            .Bind<EnemySpawnerSettingsData>()
            .FromScriptableObject(_gameplayConfig.EnemySpawnerSettingsData)
            .AsSingle();

        Container
            .Bind<EnemyGlobalData>()
            .FromScriptableObject(_gameplayConfig.EnemyCommonData)
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