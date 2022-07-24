
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Environment.Models;
using GameControllers.Services;
using Item;
using Item.Models;
using Extensions;
using Zenject;

namespace Environment
{
    public class ItemObjectLayer : MonoBehaviourLayer
    {
        private ItemObject.Factory itemObjectFactory;
        private IItemObjectService itemService;
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
                              LayerCollider.Factory _layerColliderFactory)
        {
            this.InitiliseMonoLayer(_layerColliderFactory, new Vector2(MonoBehaviourLayer.MAP_WIDTH, MonoBehaviourLayer.MAP_HEIGHT), "ItemLayer");
            this.itemObjectFactory = _itemObjectFactory;
            this.itemService = _itemService;

            this.itemService.itemObseravable.Subscribe(items =>
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
            });
            this.subscriptions.Add(this.itemService.onItemStoreOrSupplyTrigger.SubscribeQuietly(item =>
            {
                if (item != null) this.DeleteItemObject(item.ID);
            }));
            this.subscriptions.Add(this.itemService.onItemPickupOrDropTrigger.SubscribeQuietly(item =>
            {
                if (item != null && this.itemObjects.Find(obj => { return obj.itemObjectModel.ID == item.ID; }) == null)
                {
                    this.itemObjects.Add(this.CreateItemObject(item));
                }
            }));

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