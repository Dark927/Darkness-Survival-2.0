using UnityEngine;
using Zenject;

namespace Settings.Global
{
    public class GlobalGameSettingsInstaller : MonoInstaller
    {
        #region Methods

        public override void InstallBindings()
        {
            Container
                .Bind<Canvas>()
                .FromComponentInHierarchy()
                .AsSingle();
        }

        #endregion
    }
}
