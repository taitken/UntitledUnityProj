
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
        private IList<ItemObjectModel> itemObjectModels = new List<ItemObjectModel>();
        private IList<ItemObject> itemObjects = new List<ItemObject>();

        [Inject]
        public void Construct(IItemObjectService _itemService,
                                ItemObject.Factory _itemObjectFactory)
        {
            this.InitiliseMonoLayer();
            this.itemObjectFactory = _itemObjectFactory;
            this.itemService = _itemService;

            this.itemService.itemSubscribable.Subscribe(items =>
            {
                IList<ItemObjectModel> newItems = items.GetNewModels(this.itemObjectModels);
                newItems.ForEach(items => { this.createItemObject(items); });
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