using System;
using Zenject;
using GameControllers.Services;
using GameControllers.Models;
using Environment;
using UnityEngine;

public class GameInstaller : MonoInstaller
{
    public GameObject MineableHunkPrefab;
    public GameObject OrderIconPrefab;
    public override void InstallBindings()
    {
        Container.Bind<IUnitActionService>().To<UnitActionService>().AsSingle();
        Container.BindFactory<Vector3,  MineableHunk, MineableHunk.Factory >().FromComponentInNewPrefab(MineableHunkPrefab);
        Container.BindFactory<UnitOrderModel, OrderIcon, OrderIcon.Factory >().FromComponentInNewPrefab(OrderIconPrefab);
    }
}