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
using Building;
using Building.Models;
using Item.Models;
using Item;
using UI;
using UI.Services;
using UI.Models;
using Unit.Models;
using UtilityClasses;

public class GameInstaller : MonoInstaller
{
    public GameObject MineableBlockPrefab;
    public GameObject WorldCharacter;
    public GameObject OrderIconPrefab;
    public GameObject CharacterPathLine;
    public GameObject ItemObject;
    public GameObject ObjectContextWindow;
    public GameObject ItemList;
    public GameObject LayerCollider;
    public GameObject BuildSiteObject;
    public GameObject OrderSelectionPrefab;
    public MineableBlockAssetController mineableBlockAssetController;
    public BuildingAssetController BuildingAssetController;
    public ContextAssetFactory ContextAssetFactory;
    public ObjectPanelAssetFactory ObjectPanelAssetFactory;
    public ItemAssetController ItemAssetController;
    public ItemObjectLayer ItemObjectLayer;
    public MouseIconController mouseIconController;
    public override void InstallBindings()
    {
        // Singleton Setup
        MouseIconSingleton.mouseTextures = mouseIconController.cursorTexures;

        // Services
        Container.Bind<IUnitOrderService>().To<UnitOrderService>().AsSingle();
        Container.Bind<IUnitService>().To<UnitService>().AsSingle();
        Container.Bind<IPathFinderService>().To<PathFinderService>().AsSingle();
        Container.Bind<IEnvironmentService>().To<EnvironmentService>().AsSingle().OnInstantiated<EnvironmentService>((ctx, service) => { service.SetMineableBlockAssetController(mineableBlockAssetController); });
        Container.Bind<IBuildingService>().To<BuildingService>().AsSingle().OnInstantiated<BuildingService>((ctx, service) => { service.SetBuildingAssetController(BuildingAssetController); });
        Container.Bind<IItemObjectService>().To<ItemObjectService>().AsSingle().OnInstantiated<ItemObjectService>((ctx, service) =>
        {
            service.SetItemObjectHook(() => { return this.ItemObjectLayer.GetItemObjects(); });
            service.SetItemAssetController(ItemAssetController);
        });

        // World Objects
        Container.BindFactory<UnitModel, WorldCharacter, WorldCharacter.Factory>().FromComponentInNewPrefab(WorldCharacter);
        Container.BindFactory<MineableObjectModel, MineableBlock, MineableBlock.Factory>().FromComponentInNewPrefab(MineableBlockPrefab);
        Container.BindFactory<UnitOrderModel, OrderIcon, OrderIcon.Factory>().FromComponentInNewPrefab(OrderIconPrefab);
        Container.BindFactory<ItemObjectModel, ItemObject, ItemObject.Factory>().FromComponentInNewPrefab(ItemObject);
        Container.BindFactory<IList<Vector3>, CharacterPathLine, CharacterPathLine.Factory>().FromComponentInNewPrefab(CharacterPathLine);
        Container.BindFactory<Vector2, string, IList<Action>, IList<Action<DragEventModel>>, LayerCollider, LayerCollider.Factory>().FromComponentInNewPrefab(LayerCollider);
        Container.BindFactory<BuildSiteModel, BuildSiteObject, BuildSiteObject.Factory>().FromComponentInNewPrefab(BuildSiteObject);
        Container.BindFactory<Vector3Int, Vector3, OrderSelection, OrderSelection.Factory>().FromComponentInNewPrefab(OrderSelectionPrefab);
        Container.BindFactory<ItemListModel, ItemList, ItemList.Factory>().FromComponentInNewPrefab(ItemList);


        // Building Objects

        // UI
        Container.Bind<IUiPanelService>().To<UiPanelService>().AsSingle().OnInstantiated<UiPanelService>((ctx, service) =>
        {
            service.SetContextAssetFactory(ContextAssetFactory);
            service.SetPanelAssetFactory(ObjectPanelAssetFactory);
        }); ;
    }
}