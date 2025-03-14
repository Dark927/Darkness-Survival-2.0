
using System.IO;
using Characters.Interfaces;
using Gameplay.Components;
using Settings.Global;
using UnityEngine;
using Utilities.Json;
using Zenject;

namespace Gameplay.Stage
{
    public class StageProgressService : IService
    {
        private StageProgress _stageProgress;
        private GameTimer _timer;

        [Inject]
        public void Construct(GameTimer gameTimer)
        {
            _timer = gameTimer;
        }

        /// <summary>
        /// Collects the current stage progress and optionally saves it to a file.
        /// </summary>
        /// <param name="save">save requested progress to the file for later use</param>
        /// <returns>stage progress struct</returns>
        public StageProgress CollectStageProgress(bool save = true)
        {
            PlayerService playerService = ServiceLocator.Current.Get<PlayerService>();

            if (playerService != null)
            {
                ICharacterLogic character = playerService.GetCharacter();

                if (character != null)
                {
                    _stageProgress.CharacterLevel = character.Level.ActualLevel;
                }
            }

            _stageProgress.SurvivedTime = _timer.CurrentTime;

            if (save)
            {
                JsonHelper.SaveToJson(_stageProgress, GameSavePaths.StageProgressFilePath);
            }

            return _stageProgress;
        }

        /// <summary>
        /// tries to load the previous stage progress
        /// </summary>
        /// <param name="progress">loaded stage progress if successful</param>
        /// <returns>true if the progress was loaded successfully, otherwise false</returns>
        public bool TryCollectSavedStageProgress(out StageProgress progress)
        {
            return JsonHelper.TryLoadFromJson(GameSavePaths.StageProgressFilePath, out progress);
        }

        public void IncrementKills()
        {
            _stageProgress.KilledEnemies += 1;
        }
    }
}
