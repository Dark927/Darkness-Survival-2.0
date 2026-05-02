using System;
using Gameplay.Components;
using Settings.Global;
using Settings.Global.Audio;
using World.Environment;
using Zenject;
using Gameplay.Components.Enemy;
using Gameplay.Stage;
using Characters.Player;
using Settings.CameraManagement;

namespace Stage
{
    public class StageManager : SceneServiceManagerBase, IEventListener
    {
        #region Fields 

        private StageMusicSetData _stageMusicSet;
        private StagePostProcessService _postProcessService;

        private GameStateService _gameStateService;
        private StageProgressService _stageProgress;
        private PlayerService _playerService;
        private CameraService _cameraService;

        private GameTimer _stageTimer;
        private EnemySpawner _enemySpawner;
        private DayManager _dayManager;

        private GameAudioService _audioService;

        private bool _stageActive = false;
        private bool _isIntroActive = false;

        #endregion


        #region Properties

        public bool CanUpdateStage => _stageActive && !_isIntroActive;

        #endregion


        #region Methods

        #region Init

        [Inject]
        public void Construct(
            StageMusicSetData stageMusicSet,
            GameTimer timer,
            EnemySpawner enemySpawner,
            DayManager dayManager)
        {
            _stageTimer = timer;
            _stageMusicSet = stageMusicSet;
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
            _playerService = ServiceLocator.Current.Get<PlayerService>();
            _cameraService = ServiceLocator.Current.Get<CameraService>();
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

            if (_stageMusicSet != null)
            {
                _audioService.MusicPlayer.AddMusicClips(_stageMusicSet.GetAllMusicData());
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
            if (CanUpdateStage)
            {
                _stageTimer.UpdateTime();
                _dayManager.UpdateDayState(_stageTimer.ElapsedTime);
            }
        }

        private void HandleGameEvent(GameEventArgs args)
        {
            switch (args.EventType)
            {
                case GameStateEventType.StageIntroStarted:
                    StartStageIntro();
                    break;

                case GameStateEventType.StageIntroFinishing:
                    FinishStageIntro();
                    break;

                case GameStateEventType.StageStarted:
                    StartStage();
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
            _playerService.PerformCharactersSpecificAction(characters =>
            {
                foreach (var character in characters)
                {
                    character.ReactToDayStateChange(args.DayTimeType);
                }
            });
        }

        private void StartStageIntro()
        {
            _cameraService.ResetCamera();
            _dayManager.UpdateDayState(_stageTimer.ElapsedTime);
            _audioService.MusicPlayer.StartPlaylist(MusicType.StageIntro, skipCurrentPlaylist: true, isLoop: true);
            _isIntroActive = true;
        }

        private void StartStage()
        {
            if (_playerService.TryGetPlayer(out PlayerCharacterController player))
            {
                _cameraService.FollowPlayer(player);
            }

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

        private void FinishStageIntro()
        {
            _isIntroActive = false;
        }

        private void DeactivateStage()
        {
            _stageActive = false;
        }

        #endregion
    }
}
