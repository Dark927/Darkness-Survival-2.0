using System;
using UnityEngine;
using Gameplay.Components;
using Gameplay.Generation;
using Gameplay.Tile;
using Zenject;
using Gameplay.Components.Enemy;
using Gameplay.Data;

public class StageInstaller : MonoInstaller
{
    [Header("World - Settings")]
    [SerializeField] private WorldGeneration _worldGenerationType = WorldGeneration.Scrolling;
    [SerializeField] private WorldGenerationData _worldGenerationSettings;
    [SerializeField] private GameObjectsContainer _worldChunksContainer;

    [Header("Enemy Management - Settings")]
    [SerializeField] private EnemyManagementData _enemyManagementData;


    public override void InstallBindings()
    {
        Container
            .Bind<GameTimer>()
            .FromComponentInHierarchy()
            .AsSingle();

        BindWorld();
        BindEnemyManagement();
    }

    private void BindEnemyManagement()
    {
        Container
            .Bind<EnemyManagementData>()
            .FromInstance(_enemyManagementData)
            .AsSingle();
    }

    private void BindWorld()
    {
        // ----------------------------------------------------------
        // ! Binding World Generation strategy inside Scene Context
        // ----------------------------------------------------------

        ConfigureChunksContainer();

        Container
            .Bind<WorldGenerationData>()
            .FromScriptableObject(_worldGenerationSettings)
            .AsSingle();

        Type worldGenerationType = WorldGenerator.GetGenerationStrategyType(_worldGenerationType);

        Container
            .Bind<GenerationStrategy>()
            .To(worldGenerationType)
            .AsSingle()
            .WithArguments(_worldChunksContainer);

        // ----------------------------------------------------------
    }

    private void ConfigureChunksContainer()
    {
        if (_worldChunksContainer == null)
        {
            WorldGenerator worldGenerator = FindObjectOfType<WorldGenerator>();

            if (worldGenerator == null)
            {
                throw new NullReferenceException($" # Error : {nameof(WorldGenerator)} component is not available in the scene!");
            }
            _worldChunksContainer = worldGenerator.GetComponentInChildren<GameObjectsContainer>();
        }
    }
}