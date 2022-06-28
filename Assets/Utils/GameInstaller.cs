using System;
using Zenject;
using GameControllers.Services;
using Environment;
using UnityEngine;

public class GameInstaller : MonoInstaller
{
    public GameObject MineableHunkPrefab;
    public override void InstallBindings()
    {
        Container.Bind<IUnitActionService>().To<UnitActionService>().AsSingle();
        Container.BindFactory<Vector3,  MineableHunk, MineableHunk.Factory >().FromComponentInNewPrefab(MineableHunkPrefab);
    }
}