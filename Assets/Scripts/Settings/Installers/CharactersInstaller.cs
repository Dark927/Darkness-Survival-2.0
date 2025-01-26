using Characters.Player;
using Zenject;

public sealed class CharactersInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container
            .Bind<ICharacterLogic>()
            .To<Nero>()
            .FromComponentInHierarchy()
            .AsSingle();
    }
}