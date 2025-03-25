using System;
using Gameplay.Components;
using Gameplay.Components.Enemy;
using Gameplay.Data;
using Gameplay.Generation;
using Gameplay.Stage;
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
        [SerializeField] private Transform _stageComponentsContainer;


        [Header("Post Process - Settings")]
        [SerializeField] private Volume _stageVolume;
        [SerializeField] private StagePostProcessData _postProcessData;

        [Header("World - Settings")]
        [SerializeField] private WorldGeneration _worldGenerationType = WorldGeneration.Scrolling;
        [SerializeField] private WorldGenerationData _worldGenerationSettings;
        [SerializeField] private GameObjectsContainer _worldChunksContainer;
        [Space]
        [SerializeField] private ShadowSettings _shadowSettings;
        [SerializeField] private DayStatesSetData _dayStatesSetData;

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
                .AsSingle()
                .NonLazy();

            BindPostProcess();
            BindWorld();
            BindEnemyManagement();
        }

        private void BindPostProcess()
        {
            Container
                .Bind<Volume>()
                .FromInstance(_stageVolume)
                .AsSingle()
                .NonLazy();

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
                .AsSingle()
                .NonLazy();

            Type worldGenerationType = WorldGenerator.GetGenerationStrategyType(_worldGenerationType);

            Container
                .Bind<GenerationStrategy>()
                .To(worldGenerationType)
                .AsSingle()
                .WithArguments(_worldChunksContainer)
                .NonLazy();

            // ----------------------------------------------------------

            Container
                .Bind<MusicData>()
                .FromInstance(_stageMusicData)
                .AsSingle()
                .NonLazy();

            BindDayState();
        }

        private void BindDayState()
        {
            StageLight stageLight = FindAnyObjectByType<StageLight>();

            Container
                .Bind<StageLight>()
                .FromInstance(stageLight)
                .AsSingle()
                .NonLazy();


            // Day Manager creation, configuration and bind

            GameObject dayManagerObject = new GameObject(nameof(DayManager), typeof(DayManager));
            dayManagerObject.transform.SetParent(_stageComponentsContainer, false);
            DayManager dayManager = dayManagerObject.GetComponent<DayManager>();
            dayManager.SetDayManagerSettings(stageLight, _dayStatesSetData);

            Container
                .Bind<DayManager>()
                .FromInstance(dayManager)
                .AsSingle()
                .NonLazy();


            Container
                .Bind<ShadowSettings>()
                .FromScriptableObject(_shadowSettings)
                .AsSingle()
                .Lazy();
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
