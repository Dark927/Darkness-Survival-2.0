using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GlobalSettingsInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container
            .Bind<CinemachineVirtualCamera>()
            .FromComponentInHierarchy()
            .AsSingle();
    }
}
