using System;
using Zenject;
using GameControllers.Services;
using GameControllers.Models;
using Environment;
using Environment.Models;
using UnityEngine;
using System.Collections.Generic;
using Characters.Utils;

public class GameInstaller : MonoInstaller
{
    public GameObject MineableHunkPrefab;
    public GameObject OrderIconPrefab;
    public GameObject CharacterPathLine;
    public override void InstallBindings()
    {
        Container.Bind<IUnitActionService>().To<UnitActionService>().AsSingle();
        Container.Bind<IEnvironmentService>().To<EnvironmentService>().AsSingle();
        Container.Bind<IPathFinderService>().To<PathFinderService>().AsSingle();
        Container.BindFactory<MineableObjectModel,  MineableHunk, MineableHunk.Factory >().FromComponentInNewPrefab(MineableHunkPrefab);
        Container.BindFactory<UnitOrderModel, OrderIcon, OrderIcon.Factory >().FromComponentInNewPrefab(OrderIconPrefab);
        Container.BindFactory<IList<Vector3>, CharacterPathLine, CharacterPathLine.Factory >().FromComponentInNewPrefab(CharacterPathLine);
    }
}