using Cinemachine;
using World.Tile;
using Zenject;

public class SceneInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container
            .Bind<CinemachineVirtualCamera>()
            .FromComponentInHierarchy()
            .AsSingle();


        // ----------------------------------------------------------
        // ! Binding World Generation strategy inside Scene Context
        // ----------------------------------------------------------

        Container
            .Bind<GenerationStrategy>()
            .To<WorldScrolling>()
            .AsSingle();

        // ----------------------------------------------------------
    }
}