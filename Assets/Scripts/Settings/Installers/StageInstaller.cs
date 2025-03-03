using System;
using Dark.Environment;
using UnityEngine;
using World.Components;
using World.Data;
using World.Generation;
using World.Tile;
using Zenject;

public class StageInstaller : MonoInstaller
{
    [Header("Main parameters")]
    [SerializeField] private WorldGeneration _worldGenerationType = WorldGeneration.Scrolling;
    [SerializeField] private GenerationSettings _worldGenerationSettings;
    [SerializeField] private GameObjectsContainer _worldChunksContainer;
    [SerializeField] private ShadowSettings _shadowSettings;


    public override void InstallBindings()
    {
        Container
            .Bind<GameTimer>()
            .FromComponentInHierarchy()
            .AsSingle();

        // ----------------------------------------------------------
        // ! Binding World Generation strategy inside Scene Context
        // ----------------------------------------------------------

        ConfigureChunksContainer();

        Container
            .Bind<GenerationSettings>()
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
