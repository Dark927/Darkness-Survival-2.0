using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Assets.Scripts.Settings.Global.Audio;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Settings.AssetsManagement;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Utilities.ErrorHandling;
using Utilities.UI;

namespace Settings.Global.Audio
{
    public class MusicPlayer : IDisposable
    {
        #region Fields 

        private const float InstantTransitionTime = 0f;

        private AudioSource _mainMusicSource;
        private AudioSource _secondarySource;

        private AudioFXHandler _fxHandler;
        private MusicPlayerSettings _settings;

        private Dictionary<MusicType, List<AssetReferenceT<AudioClip>>> _clipsOST;
        private Queue<AssetReferenceT<AudioClip>> _musicQueue;

        private CancellationTokenSource _mainPlaylistCts;
        private bool _isMainQueuePaused;

        private Tween _secondarySongAnimation;

        #endregion


        #region Properties 

        public bool IsMainSourceBusy => (_mainMusicSource != null) && _mainMusicSource.isPlaying || _isMainQueuePaused;

        #endregion


        #region Methods 

        #region Init


        public MusicPlayer(MusicPlayerSettings settings, Transform audioSourcesContainer)
        {
            _settings = settings;
            _fxHandler = new AudioFXHandler();
            _mainMusicSource = CreateAndConfigureAudioSource(audioSourcesContainer);
            _secondarySource = CreateAndConfigureAudioSource(audioSourcesContainer);
        }

        private AudioSource CreateAndConfigureAudioSource(Transform container)
        {
            AudioSource audioSource = container.gameObject.AddComponent<AudioSource>();
            ConfigureMusicSource(audioSource);
            return audioSource;
        }

        private void ConfigureMusicSource(AudioSource musicSource)
        {
            musicSource.reverbZoneMix = _settings.ReverbZoneMix;
            musicSource.spatialBlend = 0f;
            musicSource.playOnAwake = false;
            musicSource.loop = false;
        }

        public void Dispose()
        {
            Stop();
        }


        #endregion

        public void PlaySong(AudioClip targetClip, float fadeInOutDuration = 1f, float startTime = 0f)
        {
            float halfFadeDuration = fadeInOutDuration / 2f;

            // Fade out the current music
            _fxHandler.FadeOut(_mainMusicSource, halfFadeDuration, () =>
            {
                _mainMusicSource.clip = targetClip;
                _mainMusicSource.time = startTime;
                _mainMusicSource.Play();

                _fxHandler.FadeIn(_mainMusicSource, 0f, _settings.MaxVolume, halfFadeDuration);
            });
        }

        public void PauseMainSong(float fadeDuration = 0f)
        {
            _isMainQueuePaused = true;

            if (!_mainMusicSource.isPlaying)
            {
                return;
            }

            _fxHandler.FadeOut(_mainMusicSource, fadeDuration, () =>
            {
                _mainMusicSource.Pause();
            });
        }

        #region Music Pause & Resume 

        public void InterruptWithPauseTypeSong()
        {
            if (_clipsOST.TryGetValue(MusicType.PauseMenu, out var availableSongs))
            {
                int randomSongIndex = UnityEngine.Random.Range(0, availableSongs.Count());
                PlayInterruptingSong(availableSongs[randomSongIndex], _settings.SongInterruptionTransitionTime).Forget();
            }
            else
            {
                PauseMainSong(InstantTransitionTime);
            }
        }

        private async UniTask PlayInterruptingSong(AssetReferenceT<AudioClip> interruptClip, float fadeDuration = 1f)
        {
            PauseMainSong(InstantTransitionTime);
            var clip = await LoadClipAsync(interruptClip);

            TweenHelper.KillTweenIfActive(_secondarySongAnimation, false);
            _secondarySource.clip = clip;
            _secondarySource.Play();
            _fxHandler.FadeIn(_secondarySource, 0f, _settings.MaxVolume, fadeDuration);
        }

        private void StopSecondaryMusic(float fadeOutDuration = 1f)
        {
            if (!_secondarySource.isPlaying)
            {
                return;
            }

            TweenHelper.KillTweenIfActive(_secondarySongAnimation, false);

            Action onCompleteCallback = () =>
            {
                _secondarySource.Stop();
                _secondarySource.clip = null;
                _secondarySongAnimation = null;
            };


            if (_fxHandler.TryFadeOutPlayingSong(_secondarySource, fadeOutDuration, out var animation))
            {
                _secondarySongAnimation = animation;
                animation.OnComplete(() => onCompleteCallback());
            }
            else
            {
                onCompleteCallback();
            }
        }

        public void ResumeMainSong()
        {
            if (!_isMainQueuePaused)
            {
                return;
            }
            StopSecondaryMusic(_settings.SongInterruptionTransitionTime / 2f);

            _mainMusicSource.volume = 0f;
            _mainMusicSource.UnPause();
            _fxHandler.FadeIn(_mainMusicSource, _mainMusicSource.volume, _settings.MaxVolume, _settings.SongInterruptionTransitionTime / 2f);

            DropPause();
        }

        public void Stop()
        {
            if (_mainMusicSource.clip == null)
            {
                return;
            }

            _mainMusicSource.Stop();
            _mainMusicSource.clip = null;

            StopMainPlaylist();
            StopSecondaryMusic();
            DropPause();
        }


        private void StopMainPlaylist()
        {
            if (_mainPlaylistCts == null)
            {
                return;
            }

            _mainPlaylistCts.Cancel();
            _mainPlaylistCts.Dispose();
            _mainPlaylistCts = null;
        }

        private void DropPause()
        {
            _isMainQueuePaused = false;
        }

        #endregion


        public void AddMusicClips(params MusicData[] musicDataList)
        {
            _clipsOST ??= new();

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
            if ((_clipsOST == null) || (_mainMusicSource == null))
            {
                ErrorLogger.LogWarning("# The OST list is empty or Music Source is null, can not play it! - " + nameof(GameAudioService)
                                + $"\nOST clips list -> {_clipsOST}"
                                + $"\nMusicSource -> {_mainMusicSource}");
                return;
            }

            if (_mainMusicSource.isPlaying && !skipCurrentPlaylist)
            {
                return;
            }

            Stop();
            UpdateMusicQueue(type);

            if (_mainPlaylistCts == null || _mainPlaylistCts.IsCancellationRequested)
            {
                _mainPlaylistCts = new CancellationTokenSource();
            }

            PlayMusicQueue(_mainPlaylistCts.Token).Forget();
        }


        public async UniTask PlayMusicQueue(CancellationToken token = default)
        {
            AudioClip nextClip;

            while (_musicQueue.Count > 0 && !token.IsCancellationRequested)
            {
                if (!_isMainQueuePaused)
                {
                    nextClip = await LoadClipAsync(_musicQueue.Dequeue());

                    if (token.IsCancellationRequested)
                    {
                        break;
                    }

                    PlaySong(nextClip, _settings.SongTransitionTime);
                    await UniTask.WaitUntil(() => !IsMainSourceBusy && Application.isFocused, cancellationToken: token);
                }
                else
                {
                    await UniTask.WaitUntil(() => !_isMainQueuePaused, cancellationToken: token);
                }
            }
        }

        private void UpdateMusicQueue(MusicType musicType)
        {
            if (_clipsOST == null || !_clipsOST.ContainsKey(musicType))
            {
                ErrorLogger.LogWarning($"# The OST list is null or does not contain this music type {musicType}! - {nameof(GameAudioService)}");
                return;
            }

            _musicQueue = new Queue<AssetReferenceT<AudioClip>>(_clipsOST[musicType].Distinct()); // ToDo : check ordering
        }

        private async UniTask<AudioClip> LoadClipAsync(AssetReferenceT<AudioClip> musicRef)
        {
            var clipLoadHandle = AddressableAssetsHandler.Instance.TryLoadAssetAsync<AudioClip>(musicRef);
            await clipLoadHandle.Task;

            AddressableAssetsHandler.Instance.Cleaner.SubscribeOnCleaning(AddressableAssetsCleaner.CleanType.SceneSwitch, clipLoadHandle);
            return clipLoadHandle.Result;
        }

        #endregion
    }
}
