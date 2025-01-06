using Settings;
using System;
using UnityEngine;
using Zenject;
using Utilities.ErrorHandling;

public class GameSettingsInstaller : MonoInstaller
{
    private GlobalGameConfig _gameConfig;

    public override void InstallBindings()
    {
        _gameConfig = Resources.Load<GlobalGameConfig>(ResourcesPath.GlobalGameConfigPath);

        try
        {
            if (_gameConfig == null)
            {
                throw new NullReferenceException(nameof(GlobalGameConfig));
            }

            Container.Bind<ObjectPoolSettings>().FromScriptableObject(_gameConfig.PoolsSettings).AsSingle();
            Container.Bind<EnemySpawnSettings>().FromScriptableObject(_gameConfig.EnemySpawnSettings).AsSingle();
        }
        catch (Exception e)
        {
            ErrorLogger.LogComponentIsNull(LogOutputType.Console, gameObject.name, e.Message);
        }
    }
}
