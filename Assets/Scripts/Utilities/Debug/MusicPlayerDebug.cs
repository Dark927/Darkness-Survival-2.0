using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Utilities.Json;

#if UNITY_EDITOR
namespace Settings.Global.Audio
{
    public class MusicPlayerDebug
    {
        [System.Serializable]
        public class SongInfo
        {
            public string CurrentSongName;
            public float CurrentSongLength;
            public float CurrentSongTime;
            public List<string> MusicQueue;
        }

        private MusicPlayer _musicPlayer;
        private AudioSource _mainMusicSource;
        private Queue<AssetReferenceT<AudioClip>> _musicQueue;


        public MusicPlayerDebug(MusicPlayer musicPlayer)
        {
            _musicPlayer = musicPlayer;

            ConfigureDebugFields();
        }

        private void ConfigureDebugFields()
        {
            var fieldInfo = typeof(MusicPlayer).GetField(nameof(_mainMusicSource), BindingFlags.NonPublic | BindingFlags.Instance);

            if (fieldInfo != null)
            {
                _mainMusicSource = (AudioSource)fieldInfo.GetValue(_musicPlayer);
            }

            fieldInfo = typeof(MusicPlayer).GetField(nameof(_musicQueue), BindingFlags.NonPublic | BindingFlags.Instance);

            if (fieldInfo != null)
            {
                _musicQueue = (Queue<AssetReferenceT<AudioClip>>)fieldInfo.GetValue(_musicPlayer);
            }
        }

        private void SaveMusicInfoToJson()
        {
            if (_mainMusicSource == null)
            {
                return;
            }

            var currentSongInfo = new SongInfo
            {
                CurrentSongName = _mainMusicSource.isPlaying ? _mainMusicSource.clip.name : "not playing",
                CurrentSongLength = _mainMusicSource.isPlaying ? _mainMusicSource.clip.length : -999,
                CurrentSongTime = _mainMusicSource.isPlaying ? _mainMusicSource.time : -999,
                MusicQueue = _musicQueue != null ? new List<string>(_musicQueue.Select(clip => clip.editorAsset.name)) : null,
            };

            // Create the file path for the desktop
            string desktopPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop);
            string filePath = Path.Combine(desktopPath, "MusicInfo.json");

            JsonHelper.SaveToJsonAsync(currentSongInfo, filePath, true).Forget();
        }

        private async UniTask LogMusicPlayer()
        {
            while (true)
            {
                SaveMusicInfoToJson();
                await UniTask.WaitForSeconds(1f);
            }
        }

        public void OnGUI()
        {
            float buttonWidth = 75f;
            float buttonHeight = 15f;

            // Pause button
            if (GUI.Button(new Rect(10, 70, buttonWidth, buttonHeight), "Pause"))
            {
                PauseMainSong();
            }

            // Continue button
            if (GUI.Button(new Rect(10, 90, buttonWidth, buttonHeight), "Continue"))
            {
                ResumeMainSong();
            }

            // Skip button
            if (GUI.Button(new Rect(10, 110, buttonWidth, buttonHeight), "Skip"))
            {
                SkipMusic();
            }

            DisplayMusicInfo();
        }

        private void ResumeMainSong()
        {
            _musicPlayer.ResumeMainSong();
        }

        private void PauseMainSong()
        {
            _musicPlayer.PauseMainSong();
        }

        private void SkipMusic()
        {
            if (_mainMusicSource.isPlaying)
            {
                _mainMusicSource.time = _mainMusicSource.clip.length * 0.99f;
            }
        }

        private void DisplayMusicInfo()
        {
            if (_mainMusicSource.clip != null)
            {
                // Format time in mm:ss
                string currentTimeFormatted = FormatTime(_mainMusicSource.time);
                string totalTimeFormatted = FormatTime(_mainMusicSource.clip.length);

                GUI.Label(new Rect(10, 320, 300, 30),
                    $"OST: {_mainMusicSource.clip.name} | Time: {currentTimeFormatted} / {totalTimeFormatted}");
            }
        }

        // Helper method to format time (e.g., 01:20 / 03:65)
        private string FormatTime(float time)
        {
            int minutes = Mathf.FloorToInt(time / 60f);
            int seconds = Mathf.FloorToInt(time % 60f);
            return string.Format("{0:D2}:{1:D2}", minutes, seconds);
        }

#endif
    }
}
