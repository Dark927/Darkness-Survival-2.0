using Gameplay.Stage;
using TMPro;
using UnityEngine;

namespace UI
{
    public class GameplayInfoMenuUI : BasicMenuUI
    {
        #region Fields 

        [SerializeField] private TextMeshProUGUI _characterLevelText;
        [SerializeField] private TextMeshProUGUI _killedEnemiesText;
        [SerializeField] private TextMeshProUGUI _survivedTimeText;

        #endregion


        #region Properties

        #endregion


        #region Methods

        #region Init



        #endregion

        public void SetProgressStats(StageProgress progress)
        {
            _characterLevelText.text = progress.CharacterLevel.ToString("00");
            _killedEnemiesText.text = progress.KilledEnemies.ToString("000");
            _survivedTimeText.text = progress.SurvivedTime.ToString();
        }


        #endregion
    }
}
