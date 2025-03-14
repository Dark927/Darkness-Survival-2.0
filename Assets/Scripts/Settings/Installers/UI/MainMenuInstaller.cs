using UI;
using Zenject;

namespace Settings.Installers
{
    public class MainMenuInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            GamePanelManagerUI panelManager = FindAnyObjectByType<GamePanelManagerUI>();

            Container
                .Bind<GamePanelManagerUI>()
                .FromInstance(panelManager)
                .AsSingle();
        }
    }
}
