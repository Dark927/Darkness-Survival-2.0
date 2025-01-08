using Zenject;

public class CharactersInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container
            .Bind<IPlayerLogic>()
            .To<Nero>()
            .FromComponentInHierarchy()
            .AsSingle();
    }
}