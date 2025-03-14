using UI;
using UnityEngine;
using Utilities.Attributes;
using Zenject;

namespace Settings.Global
{
    public class GlobalGameSettingsInstaller : MonoInstaller
    {
        [CustomHeader("Game Panel Manager UI - Settings")]
        [SerializeField] private Canvas _targetCanvas;
        [SerializeField] private GamePanelDataUI _panelData;


        #region Methods

        public override void InstallBindings()
        {
            GamePanelManagerUI panelManager = FindAnyObjectByType<GamePanelManagerUI>();

            if (panelManager == null)
            {
                GameObject panelManagerObj = new GameObject("GamePanelManagerUI", new System.Type[] { typeof(GamePanelManagerUI) });
                panelManager = panelManagerObj.GetComponent<GamePanelManagerUI>();
            }

            panelManager.Initialize(Container, _targetCanvas, _panelData);

            Container
                .Bind<GamePanelManagerUI>()
                .FromInstance(panelManager)
                .AsSingle()
                .NonLazy();
        }

        #endregion
    }
}
