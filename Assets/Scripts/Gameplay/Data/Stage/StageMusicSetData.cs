using Settings.Global.Audio;
using UnityEngine;

namespace Gameplay.Stage
{
    /// <summary>
    /// this SO provides the music information for current Stage and is used to create sets of music data.
    /// </summary>
    [CreateAssetMenu(fileName = "NewStageMusicSetData", menuName = "Game/World/Data/Stage Music Set Data")]
    public class StageMusicSetData : ScriptableObject
    {
        [SerializeField] private MusicData _stageIntroTheme;
        [SerializeField] private MusicData _mainStageTheme;

        public MusicData StageIntroTheme => _stageIntroTheme;
        public MusicData MainStageTheme => _mainStageTheme;

        public MusicData[] GetAllMusicData()
        {
            return new MusicData[] { _stageIntroTheme, _mainStageTheme };
        }
    }
}
