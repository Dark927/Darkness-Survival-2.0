using System;
using Gameplay.Components;
using Gameplay.Components.Enemy;
using Gameplay.Data;
using Gameplay.Generation;
using Gameplay.Tile;
using Settings.Global;
using Settings.Global.Audio;
using UnityEngine;
using UnityEngine.Rendering;
using World.Data;
using World.Environment;
using World.Light;
using Zenject;

namespace Settings.Installers
{
    public class StageInstaller : MonoInstaller
    {
        [Header("Containers - Settings")]
        [SerializeField] private Transform _stageObjectsContainer;


        [Header("Post Process - Settings")]
        [SerializeField] private Volume _stageVolume;
        [SerializeField] private StagePostProcessData _postProcessData;

        [Header("World - Settings")]
        [SerializeField] private WorldGeneration _worldGenerationType = WorldGeneration.Scrolling;
        [SerializeField] private WorldGenerationData _worldGenerationSettings;
        [SerializeField] private GameObjectsContainer _worldChunksContainer;
        [SerializeField] private ShadowSettings _shadowSettings;


        [Header("Enemy Management - Settings")]
        [SerializeField] private GameObject _enemySpawnerPrefab;
        [SerializeField] private EnemyManagementData _enemyManagementData;

        [Header("Stage Audio - Settings")]
        [SerializeField] private MusicData _stageMusicData;

        public override void InstallBindings()
        {

            Container
                .Bind<GameTimer>()
                .FromComponentInHierarchy()
                .AsSingle();

            BindPostProcess();
            BindWorld();
            BindEnemyManagement();
        }

        private void BindPostProcess()
        {
            Container
                .Bind<Volume>()
                .FromInstance(_stageVolume)
                .AsSingle();

            Container
                .Bind<StagePostProcessData>()
                .FromInstance(_postProcessData)
                .AsSingle();
        }

        private void BindEnemyManagement()
        {
            Container.Bind<EnemySpawner>()
                 .FromComponentInNewPrefab(_enemySpawnerPrefab)
                 .UnderTransform(_stageObjectsContainer)
                 .AsSingle()
                 .NonLazy();

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

            Container
                .Bind<StageLight>()
                .FromComponentInHierarchy()
                .AsSingle();

            Container
                .Bind<MusicData>()
                .FromInstance(_stageMusicData)
                .AsSingle();

            Container
                .Bind<DayManager>()
                .FromComponentInHierarchy()
                .AsSingle();

            Container
                .Bind<ShadowSettings>()
                .FromScriptableObject(_shadowSettings)
                .AsSingle();
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
}
