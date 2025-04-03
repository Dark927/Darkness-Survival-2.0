using System;
using Characters.Player.Upgrades;
using Gameplay.Components;
using Gameplay.Components.Enemy;
using Settings;
using Settings.Global;
using Settings.Global.Audio;
using World.Environment;
using Zenject;

namespace Gameplay.Stage
{
    public class StageManager : SceneServiceManagerBase, IEventListener
    {
        #region Fields 

        private MusicData _stageMusic;
        private StagePostProcessService _postProcessService;

        private GameStateService _gameStateService;
        private StageProgressService _stageProgress;

        private GameTimer _stageTimer;
        private EnemySpawner _enemySpawner;
        private DayManager _dayManager;

        private GameAudioService _audioService;

        private bool _stageActive;

        #endregion


        #region Properties

        #endregion


        #region Methods

        #region Init

        [Inject]
        public void Construct(
            MusicData stageMusic,
            GameTimer timer,
            EnemySpawner enemySpawner,
            DayManager dayManager)
        {
            _stageTimer = timer;
            _stageMusic = stageMusic;
            _enemySpawner = enemySpawner;
            _dayManager = dayManager;
        }

        private void Awake()
        {
            CreateService<EnemyManagementService>();
            CreateService<StagePostProcessService>();
            CreateService<StageProgressService>();

            _stageProgress = new StageProgressService();
            _postProcessService = ServiceLocator.Current.Get<StagePostProcessService>();
            _gameStateService = ServiceLocator.Current.Get<GameStateService>();
        }

        private void Start()
        {
            InitServices();
            ConfigureAudio();

            _enemySpawner.Initialize();
            _dayManager.Initialize();

            _dayManager.DayStateChangeEvent.Subscribe(this);
            _dayManager.RaiseCurrentDayStateEvent();

            _gameStateService.GameEvent.Subscribe(this);
        }

        public override void Dispose()
        {
            base.Dispose();
            _dayManager.DayStateChangeEvent.Unsubscribe(this);
            _gameStateService.GameEvent.Unsubscribe(this);
        }

        private void InitServices()
        {
            foreach (var service in Services)
            {
                if (service is Settings.Global.IInitializable initService)
                {
                    initService.Initialize();
                }
            }
        }

        private void ConfigureAudio()
        {
            _audioService = ServiceLocator.Current.Get<GameAudioService>();

            if (_stageMusic != null)
            {
                _audioService.MusicPlayer.AddMusicClips(_stageMusic);
            }
        }


        #endregion

        public void Listen(object sender, EventArgs args)
        {
            switch (sender)
            {
                case GameStateService:
                    HandleGameEvent(args as GameEventArgs);
                    break;

                case DayManager:
                    HandleDayStateEvent(args as DayChangedEventArgs);
                    break;
            }
        }

        private void Update()
        {
            if (!_stageActive)
            {
                return;
            }

            _stageTimer.UpdateTime();
            _dayManager.UpdateDayState(_stageTimer.ElapsedTime);
        }

        private void HandleGameEvent(GameEventArgs args)
        {
            switch (args.EventType)
            {
                case GameStateEventType.StageStarted:
                    StageStarted();
                    break;

                case GameStateEventType.Unspecified:
                    break;

                case GameStateEventType.StageStartFinishing:
                    StageStartFinishing();
                    break;

                case GameStateEventType.StageCompletelyOver:
                    StageFinished();
                    break;

                case GameStateEventType.StagePaused:
                    DeactivateStage();
                    break;

                case GameStateEventType.StageUnpaused:
                    ActivateStage();
                    break;
            }
        }

        private void HandleDayStateEvent(DayChangedEventArgs args)
        {
            PlayerService playerService = ServiceLocator.Current.Get<PlayerService>();

            playerService.PerformCharactersSpecificAction(characters =>
            {
                foreach (var character in characters)
                {
                    character.ReactToDayStateChange(args.DayTimeType);
                }
            });
        }

        private void StageStarted()
        {
            ActivateStage();
            _enemySpawner.StartEnemySpawn();
            _audioService.MusicPlayer.StartPlaylist(MusicType.Stage);
        }

        private void StageFinished()
        {
            _postProcessService?.Grayscale?.ApplyGrayscale();
        }

        private void StageStartFinishing()
        {
            DeactivateStage();
            _enemySpawner.StopAllSpawnTasks();
        }

        private void ActivateStage()
        {
            _stageActive = true;
        }

        private void DeactivateStage()
        {
            _stageActive = false;
        }

        #endregion
    }
}
