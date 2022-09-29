
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Environment.Models;
using GameControllers.Services;
using Item;
using Item.Models;
using Extensions;
using Zenject;
using Building.Models;
using System.Linq;
using static Item.Models.ItemObjectModel;

namespace Environment
{
    public class ItemObjectLayer : MonoBehaviourLayer
    {
        private ItemObject.Factory itemObjectFactory;
        private IItemObjectService itemService;
        private IBuildingService buildingService;
        private IEnvironmentService envService;
        private IPathFinderService pfService;
        public IList<ItemObject> itemObjects = new List<ItemObject>();
        private IList<ItemObjectModel> itemObjectModels
        {
            get
            {
                return this.itemObjects.Map(item => { return item.itemObjectModel; });
            }
        }

        [Inject]
        public void Construct(IItemObjectService _itemService,
                              ItemObject.Factory _itemObjectFactory,
                              LayerCollider.Factory _layerColliderFactory,
                              IPathFinderService _pfService,
                              IEnvironmentService _envService,
                              IBuildingService _buildingService)
        {
            this.InitiliseMonoLayer(_layerColliderFactory, new Vector2(MonoBehaviourLayer.MAP_WIDTH, MonoBehaviourLayer.MAP_HEIGHT), "ItemLayer");
            this.itemObjectFactory = _itemObjectFactory;
            this.itemService = _itemService;
            this.buildingService = _buildingService;
            this.envService = _envService;
            this.pfService = _pfService;
            this.itemService.itemObseravable.Subscribe(this, this.HandleItemUpdates);
            this.itemService.onItemStoreOrSupplyTrigger.SubscribeQuietly(this, this.HandleItemStoreOrSupplyTrigger);
            this.itemService.onItemPickupOrDropTrigger.SubscribeQuietly(this, this.HandleItemPickupOrDropTrigger);
            this.buildingService.SubscribeToNewBuildingTrigger(this, this.HandleNewBuilding);
        }

        // Start is called before the first frame update
        void Start()
        {
            this.tilemap = GetComponent<Tilemap>();
            this.itemService.AddItemToWorld(new ItemObjectModel(this.envService.LocalToCell(new Vector3(8f, 8f, 0)), new ItemObjectMass(eItemType.GrunberrySeed, 1), eItemState.OnGround));
            this.itemService.AddItemToWorld(new ItemObjectModel(this.envService.LocalToCell(new Vector3(8f, 8f, 0)), new ItemObjectMass(eItemType.BlumberrySeed, 1), eItemState.OnGround));
            this.itemService.AddItemToWorld(new ItemObjectModel(this.envService.LocalToCell(new Vector3(8f, 8f, 0)), new ItemObjectMass(eItemType.LuttipodSeed, 1), eItemState.OnGround));
            this.itemService.AddItemToWorld(new ItemObjectModel(this.envService.LocalToCell(new Vector3(8f, 8f, 0)), new ItemObjectMass(eItemType.PubberbillSeed, 1), eItemState.OnGround));
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void HandleItemUpdates(IList<ItemObjectModel> items)
        {
            IList<ItemObjectModel> newItems = items.GetNewModels(this.itemObjectModels);
            newItems.ForEach(item =>
            {
                if (item.itemState == ItemObjectModel.eItemState.OnGround || item.itemState == ItemObjectModel.eItemState.OnCharacter)
                {
                    this.itemObjects.Add(this.CreateItemObject(item));
                }
            });
            IList<ItemObjectModel> itemsToRemove = items.GetRemovedModels(this.itemObjectModels);
            itemsToRemove.ForEach(itemToRemove =>
            {
                this.DeleteItemObject(itemToRemove.ID);
                this.itemObjects = this.itemObjects.Filter(itemObj => { return itemObj.itemObjectModel.ID != itemToRemove.ID; });
            });
        }

        public void HandleItemStoreOrSupplyTrigger(ItemObjectModel item)
        {
            if (item != null) this.DeleteItemObject(item.ID);
        }

        public void HandleItemPickupOrDropTrigger(ItemObjectModel item)
        {
            if (item != null && this.itemObjects.Find(obj => { return obj.itemObjectModel.ID == item.ID; }) == null)
            {
                this.itemObjects.Add(this.CreateItemObject(item));
                if(item.itemState == ItemObjectModel.eItemState.OnGround) this.MergeAllItemsAtPosition(item.position);
            }
        }

        private void HandleNewBuilding(BuildingObjectModel newBuilding)
        {
            if (newBuilding is WallBuildingModel)
            {
                Vector3Int newPos = this.MoveObjectOffInvalidPosition(this.itemObjects.Cast<MonoBaseObject>().ToList(), newBuilding.position, this.pfService.GetPathFinderMap());
                if (newPos != default(Vector3Int))
                {
                    this.MergeAllItemsAtPosition(newPos);
                }
            }
        }

        private void MergeAllItemsAtPosition(Vector3Int newPos)
        {
            IList<ItemObjectModel> itemsToDelete = new List<ItemObjectModel>();
            IList<ItemObject> objectsOnNewPos = this.itemObjects.Filter(item => { return item.itemObjectModel.position == newPos; });
            var groupedObjs = objectsOnNewPos.GroupBy(item => item.itemObjectModel.itemType);
            foreach (var group in groupedObjs)
            {
                ItemObject firstItem = group.ToList()[0];
                group.ToList().ForEach((item, index) =>
                {
                    if (index > 0)
                    {
                        firstItem.itemObjectModel.AddMass(item.itemObjectModel.mass);
                        itemsToDelete.Add(item.itemObjectModel);
                    }
                });
            }
            itemsToDelete.ForEach(item => { this.itemService.RemoveItemFromWorld(item.ID); });
        }

        public IList<ItemObject> GetItemObjects()
        {
            return this.itemObjects;
        }

        private ItemObject CreateItemObject(ItemObjectModel itemObj)
        {
            ItemObject newItem = this.itemObjectFactory.Create(itemObj);
            newItem.transform.position = this.tilemap.CellToLocal(itemObj.position);
            return newItem;
        }

        private void DeleteItemObject(long itemObjID)
        {
            ItemObject itemToDelete = this.itemObjects.Find(obj => { return obj.itemObjectModel.ID == itemObjID; });
            if (itemToDelete != null)
            {
                this.itemObjects = this.itemObjects.Filter(obj => { return obj.itemObjectModel.ID != itemObjID; });
                itemToDelete.Destroy();
            }
        }

    }
}