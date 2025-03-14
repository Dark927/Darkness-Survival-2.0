using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Settings.AssetsManagement;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Settings.Global.Audio
{
    public class GameAudioService : IService
    {
        #region Fields 

        private AudioListener _audioListener = Camera.main.GetComponent<AudioListener>();
        private AudioSource _musicSource;

        ////////////
        // ToDo : move this to the MusicPlayer
        private Dictionary<MusicType, List<AssetReferenceT<AudioClip>>> _clipsOST;
        private Dictionary<AudioClip, AsyncOperationHandle<AudioClip>> _audioClipLoadHandles;

        private Queue<AssetReferenceT<AudioClip>> _musicQueue;

        ////////////

        private MusicPlayer _musicPlayer;

        #endregion


        #region Properties

        #endregion


        #region Methods

        ///////////////////////////////////////
        // ToDo : move this to the MusicPlayer
        ////////////////////////////////////////

        public GameAudioService(AudioSource musicSource)
        {
            SetMusicSource(musicSource);
            _audioClipLoadHandles = new Dictionary<AudioClip, AsyncOperationHandle<AudioClip>>();
            _musicPlayer = new MusicPlayer(musicSource);
        }

        public void SetMusicSource(AudioSource musicSource, bool playOnAwake = false)
        {
            _musicSource = musicSource;
            _musicSource.playOnAwake = playOnAwake;
        }

        public void AddMusicClips(params MusicData[] musicDataList)
        {
            if (_clipsOST == null)
            {
                _clipsOST = new();
            }

            foreach (var musicData in musicDataList)
            {
                if (musicData != null)
                {
                    _clipsOST.TryAdd(musicData.Type, musicData.MusicList);
                }
            }
        }

        public void StartPlaylist(MusicType type, bool skipCurrentPlaylist = true)
        {
            if ((_clipsOST == null) || (_musicSource == null))
            {
                Debug.LogWarning("# The OST list is empty or Music Source is null, can not play it! - " + nameof(GameAudioService)
                                + $"\nOST clips list -> {_clipsOST}"
                                + $"\nMusicSource -> {_musicSource}");
                return;
            }

            if ((_musicSource.isPlaying && !skipCurrentPlaylist))
            {
                return;
            }

            Stop();
            _musicQueue?.Clear();

            if (!_clipsOST.TryGetValue(type, out var tracksList) || (tracksList?.Count == 0))
            {
                return;
            }

            AssetReferenceT<AudioClip> firstTrackRef = tracksList?.FirstOrDefault();

            if (firstTrackRef == null)
            {
                return;
            }

            PlayAsync(firstTrackRef).Forget();
            UpdateMusicQueue(type);
        }

        public async void PlayNext()
        {
            if ((_musicQueue != null) && (_musicQueue.Count > 0))
            {
                //Stop();
                await PlayAsync(_musicQueue.Dequeue());
            }
        }

        private async UniTask PlayAsync(AssetReferenceT<AudioClip> musicRef)
        {
            AsyncOperationHandle<AudioClip> clipLoadHandle =
                AddressableAssetsHandler.Instance.TryLoadAssetAsync<AudioClip>(musicRef);

            await clipLoadHandle.Task;

            _musicPlayer.PlaySong(clipLoadHandle.Result, 4f);

            _audioClipLoadHandles.TryAdd(clipLoadHandle.Result, clipLoadHandle);
        }

        public void Stop()
        {
            if (_musicSource.clip == null)
            {
                return;
            }

            _musicSource.Stop();
            AddressableAssetsHandler.Instance.UnloadAsset(_audioClipLoadHandles[_musicSource.clip]);
            _audioClipLoadHandles.Remove(_musicSource.clip);
            _musicSource.clip = null;
        }

        private void UpdateMusicQueue(MusicType musicType)
        {
            if (_clipsOST == null)
            {
                Debug.LogWarning("# The OST list is null! - " + nameof(GameAudioService));
                return;
            }

            _musicQueue = new Queue<AssetReferenceT<AudioClip>>();

            // Skip the first one since it's already queued
            foreach (var song in _clipsOST[musicType].Skip(1))
            {
                if (!_musicQueue.Contains(song))
                {
                    _musicQueue.Enqueue(song);
                }
            }
        }
        ///////////////////////////////////////
        // ToDo : move this to the MusicPlayer
        ///////////////////////////////////////

        #endregion
    }
}
