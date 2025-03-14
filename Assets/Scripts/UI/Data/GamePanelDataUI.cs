using Settings;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Utilities.Attributes;

namespace UI
{
    [CreateAssetMenu(fileName = "UI_GamePanel_Data", menuName = "Game/UI/Game Panel Data")]
    public class GamePanelDataUI : DescriptionBaseData
    {
        #region Fields 

        [CustomHeader("Menu Panels", 2, 0)]
        [SerializeField] private AssetReference _gameSettingsMenuRef;
        [SerializeField] private AssetReference _creditsMenuRef;

        [CustomHeader("Gameplay Panels", 2, 0)]
        [SerializeField] private AssetReference _pauseMenuAsset;
        [SerializeField] private AssetReference _gameOverMenuRef;
        [SerializeField] private AssetReference _gameWinMenuRef;

        #endregion


        #region Properties

        public AssetReference GameSettingsMenuRef => _gameSettingsMenuRef;
        public AssetReference CreditsMenuRef => _creditsMenuRef;
        public AssetReference PauseMenuRef => _pauseMenuAsset;
        public AssetReference GameOverMenuRef => _gameOverMenuRef;
        public AssetReference GameWinMenuRef => _gameWinMenuRef;

        #endregion
    }
}
