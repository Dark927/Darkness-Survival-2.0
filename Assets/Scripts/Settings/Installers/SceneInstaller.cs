using Cinemachine;
using System;
using UnityEngine;
using World.Components;
using World.Tile;
using Zenject;

public class SceneInstaller : MonoInstaller
{
    [Header("Main parameters")]
    [SerializeField] private GameObjectsContainer _worldChunksContainer;

    public override void InstallBindings()
    {
        Container
            .Bind<CinemachineVirtualCamera>()
            .FromComponentInHierarchy()
            .AsSingle();

        Container
            .Bind<GameTimer>()
            .FromComponentInHierarchy()
            .AsSingle();


        // ----------------------------------------------------------
        // ! Binding World Generation strategy inside Scene Context
        // ----------------------------------------------------------

        ConfigureChunksContainer();

        Container
            .Bind<GenerationStrategy>()
            .To<WorldScrolling>()
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