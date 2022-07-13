using System;
using Zenject;
using GameControllers;
using GameControllers.Services;
using GameControllers.Models;
using Environment;
using Environment.Models;
using UnityEngine;
using System.Collections.Generic;
using Characters;
using Characters.Utils;
using Item.Models;
using Item;
using UI;
using UI.Services;
using UI.Models;

public class GameInstaller : MonoInstaller
{
    public GameObject MineableHunkPrefab;
    public GameObject WorldCharacter;
    public GameObject OrderIconPrefab;
    public GameObject CharacterPathLine;
    public GameObject ItemObject;
    public GameObject ContextWindow;
    public GameObject LayerCollider;
    public BuildingAssetController BuildingAssetController;
    public override void InstallBindings()
    {
        // Services
        Container.Bind<IUnitOrderService>().To<UnitOrderService>().AsSingle();
        Container.Bind<IUnitService>().To<UnitService>().AsSingle();
        Container.Bind<IEnvironmentService>().To<EnvironmentService>().AsSingle();
        Container.Bind<IPathFinderService>().To<PathFinderService>().AsSingle();
        Container.Bind<IItemObjectService>().To<ItemObjectService>().AsSingle();
        Container.Bind<IContextWindowService>().To<ContextWindowService>().AsSingle();
        Container.Bind<IBuildingService>().To<BuildingService>().AsSingle().OnInstantiated<BuildingService>((ctx, service) =>{service.SetBuildingAssetController(BuildingAssetController);});

        // World Objects
        Container.BindFactory<UnitModel,  WorldCharacter, WorldCharacter.Factory >().FromComponentInNewPrefab(WorldCharacter);
        Container.BindFactory<MineableObjectModel,  MineableHunk, MineableHunk.Factory >().FromComponentInNewPrefab(MineableHunkPrefab);
        Container.BindFactory<UnitOrderModel, OrderIcon, OrderIcon.Factory >().FromComponentInNewPrefab(OrderIconPrefab);
        Container.BindFactory<ItemObjectModel, ItemObject, ItemObject.Factory >().FromComponentInNewPrefab(ItemObject);
        Container.BindFactory<IList<Vector3>, CharacterPathLine, CharacterPathLine.Factory >().FromComponentInNewPrefab(CharacterPathLine);
        Container.BindFactory<Vector2, string, Action, LayerCollider, LayerCollider.Factory >().FromComponentInNewPrefab(LayerCollider);

        // UI
        Container.BindFactory<ContextWindowModel, ContextWindow, ContextWindow.Factory >().FromComponentInNewPrefab(ContextWindow);
    }
}