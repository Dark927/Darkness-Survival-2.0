using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Settings.Global.Audio
{
    public enum MusicType
    {
        Stage,
        MainMenu,
        PauseMenu,
    }


    /// <summary>
    /// this SO provides the music information and is used to create sets of music with a description and a specific type.
    /// </summary>
    [CreateAssetMenu(fileName = "NewMusicData", menuName = "Game/Settings/Audio/Music Data")]
    public class MusicData : DescriptionBaseData
    {
        [SerializeField] private MusicType _musicType;
        [SerializeField] private List<AssetReferenceT<AudioClip>> _musicList;

        public MusicType Type => _musicType;
        public List<AssetReferenceT<AudioClip>> MusicList => _musicList;
    }
}
