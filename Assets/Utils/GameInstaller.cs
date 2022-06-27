using System;
using Zenject;
using GameControllers.Services;

public class GameInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<IUnitActionService>()
        .To<UnitActionService>()
        .AsSingle();
    }
}