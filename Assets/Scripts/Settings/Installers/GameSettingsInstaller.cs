using Settings;
using System;
using UnityEngine;
using Utilities.ErrorHandling;
using Zenject;

[CreateAssetMenu(fileName = "GameSettingsInstaller", menuName = "Installers/GameSettingsInstaller")]
public class GameSettingsInstaller : ScriptableObjectInstaller<GameSettingsInstaller>
{
    private GlobalGameConfig _gameConfig;

    public override void InstallBindings()
    {
        try
        {
            TryLoadGameConfig();
            BindSettings();
            TryBindContainer();
        }
        catch (Exception e)
        {
            ErrorLogger.LogComponentIsNull(LogOutputType.Console, name, e.Message);
        }
    }

    private void TryLoadGameConfig()
    {
        _gameConfig = Resources.Load<GlobalGameConfig>(ResourcesPath.GlobalGameConfigPath);

        if (_gameConfig == null)
        {
            throw new NullReferenceException(nameof(GlobalGameConfig));
        }
    }

    private void BindSettings()
    {
        Container
            .Bind<ObjectPoolSettings>()
            .FromScriptableObject(_gameConfig.PoolsSettings)
            .AsSingle();

        Container
            .Bind<EnemySpawnSettings>()
            .FromScriptableObject(_gameConfig.EnemySpawnSettings)
            .AsSingle();
    }

    private void TryBindContainer()
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