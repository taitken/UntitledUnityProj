
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

namespace Environment
{
    public class ItemObjectLayer : MonoBehaviourLayer
    {
        private ItemObject.Factory itemObjectFactory;
        private IItemObjectService itemService;
        private IBuildingService buildingService;
        private IEnvironmentService envService;
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
                              IEnvironmentService _envService,
                              IBuildingService _buildingService)
        {
            this.InitiliseMonoLayer(_layerColliderFactory, new Vector2(MonoBehaviourLayer.MAP_WIDTH, MonoBehaviourLayer.MAP_HEIGHT), "ItemLayer");
            this.itemObjectFactory = _itemObjectFactory;
            this.itemService = _itemService;
            this.buildingService = _buildingService;
            this.envService = _envService;
            this.itemService.itemObseravable.Subscribe(this, this.HandleItemUpdates);
            this.itemService.onItemStoreOrSupplyTrigger.SubscribeQuietly(this, this.HandleItemStoreOrSupplyTrigger);
            this.itemService.onItemPickupOrDropTrigger.SubscribeQuietly(this, this.HandleItemPickupOrDropTrigger);
            _buildingService.SubscribeToNewBuildingTrigger(this, (newBuilding) => { if (newBuilding is WallBuildingModel) this.MoveItemToOffPosition(newBuilding.position); });
        }

        // Start is called before the first frame update
        void Start()
        {
            this.tilemap = GetComponent<Tilemap>();
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
            }
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

        private void MoveItemToOffPosition(Vector3Int positionToMoveOff)
        {
            IList<ItemObject> items = this.itemObjects.Filter(itemObj => { return itemObj.itemObjectModel.position == positionToMoveOff; });
            BuildingObjectModel[,] _walls = new BuildingObjectModel[MonoBehaviourLayer.MAP_WIDTH, MonoBehaviourLayer.MAP_HEIGHT];
            MineableObjectModel[,] _mineableBlocks = this.envService.mineableObjects.Get();
            this.buildingService.buildingObseravable.Get().Filter(building => { return building is WallBuildingModel; }).ForEach(wall => { _walls[wall.position.x, wall.position.y] = wall; });

            items.ForEach(item =>
            {
                if (this.CheckIfSpotFree(positionToMoveOff.x - 1, positionToMoveOff.y, _walls, _mineableBlocks))
                    this.SetItemPosition(item, new Vector3Int(positionToMoveOff.x - 1, positionToMoveOff.y));
                if (this.CheckIfSpotFree(positionToMoveOff.x + 1, positionToMoveOff.y, _walls, _mineableBlocks))
                    this.SetItemPosition(item, new Vector3Int(positionToMoveOff.x + 1, positionToMoveOff.y));
                if (this.CheckIfSpotFree(positionToMoveOff.x, positionToMoveOff.y - 1, _walls, _mineableBlocks))
                    this.SetItemPosition(item, new Vector3Int(positionToMoveOff.x, positionToMoveOff.y - 1));
                if (this.CheckIfSpotFree(positionToMoveOff.x, positionToMoveOff.y + 1, _walls, _mineableBlocks))
                    this.SetItemPosition(item, new Vector3Int(positionToMoveOff.x, positionToMoveOff.y + 1));
            });
        }

        private bool CheckIfSpotFree(int x, int y, BuildingObjectModel[,] _walls, MineableObjectModel[,] _mineableBlocks)
        {
            return _walls[x, y] == null && _mineableBlocks[x,y] == null;
        }

        private void SetItemPosition(ItemObject _item, Vector3Int pos)
        {
            _item.transform.position = this.tilemap.CellToLocal(pos);
            _item.itemObjectModel.position = pos;
        }
    }
}