using Gameplay.Components;
using Gameplay.Components.Enemy;
using Settings.Global;
using Settings.Global.Audio;
using Zenject;

namespace Gameplay.Stage
{
    public class StageManager : SceneServiceManagerBase
    {
        #region Fields 

        private MusicData _stageMusic;

        #endregion


        #region Properties

        #endregion


        #region Methods

        #region Init

        [Inject]
        public void Construct(MusicData stageMusic)
        {
            _stageMusic = stageMusic;
        }

        private void Awake()
        {
            CreateEnemyManagementService();
        }

        private void Start()
        {
            foreach (var service in Services)
            {
                if (service is Settings.Global.IInitializable initService)
                {
                    initService.Initialize();
                }
            }

            GameAudioService audioService = ServiceLocator.Current.Get<GameAudioService>();

            if ((audioService != null) && (_stageMusic != null))
            {
                audioService.AddMusicClips(_stageMusic);
                audioService.StartPlaylist(MusicType.Stage);
            }
        }

        private void CreateEnemyManagementService()
        {
            EnemyManagementService enemyManagementService = DiContainer.Instantiate<EnemyManagementService>();
            ServiceLocator.Current.Register(enemyManagementService);
            Services.Add(enemyManagementService);
        }


        #endregion

        #endregion
    }
}
