using System;
using Dark.Environment;
using Gameplay.Components;
using Gameplay.Data;
using Gameplay.Generation;
using Gameplay.Tile;
using UnityEngine;
using World.Data;
using Zenject;

public class StageInstaller : MonoInstaller
{
    [Header("World - Settings")]
    [SerializeField] private WorldGeneration _worldGenerationType = WorldGeneration.Scrolling;
    [SerializeField] private WorldGenerationData _worldGenerationSettings;
    [SerializeField] private GameObjectsContainer _worldChunksContainer;
    [SerializeField] private ShadowSettings _shadowSettings;


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

        Container
        .Bind<DayManager>()
        .FromComponentInHierarchy()
        .AsSingle();

        Container
            .Bind<ShadowSettings>()
            .FromScriptableObject(_shadowSettings)
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
