
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
                newItems.ForEach(items =>
                {
                    this.itemObjects.Add(this.createItemObject(items));
                });
            });
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

        private ItemObject createItemObject(ItemObjectModel itemObj)
        {
            ItemObject newItem = this.itemObjectFactory.Create(itemObj);
            newItem.transform.position = this.tilemap.CellToLocal(itemObj.position);
            return newItem;
        }

    }
}