using Settings.Global;
using System;
using System.Collections.Generic;
using Zenject;

namespace Assets.Scripts.Gameplay.Interfaces
{
    public interface ISceneServiceManager : IDisposable
    {
        public DiContainer DiContainer { get; }
        public List<IService> Services { get; }
        public void Construct(DiContainer diContainer);
    }
}
