using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GameControllers.Services;
using Item.Models;
using UI;
using UnityEngine;
using Zenject;

namespace UI
{
    public class ItemListController : MonoBehaviour2
    {
        IItemObjectService itemService;
        IList<ItemList> itemLists;
        ItemList.Factory itemListFactory;

        [Inject]
        public void Construct(IItemObjectService _itemService,
                                ItemList.Factory _itemListFactory)
        {
            this.itemLists = new List<ItemList>();
            this.itemListFactory = _itemListFactory;
            this.itemService = _itemService;
            this.itemService.itemObseravable.Subscribe(this, this.HandleItemList);
        }


        public void HandleItemList(IList<ItemObjectModel> items)
        {
            Dictionary<eItemType, decimal> itemMap = itemMap = items.GroupBy(i => i.itemType).ToDictionary(i => i.Key, i => i.Sum(item => item.mass));
            itemLists.DestroyAll();
            foreach (KeyValuePair<eItemType, decimal> item in itemMap)
            {
                ItemList newItemList = this.itemListFactory.Create(new ItemListModel(item.Key, item.Value, this.itemService.GetItemSprite(item.Key)));
                newItemList.GetComponent<RectTransform>().SetParent(this.transform);
                newItemList.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
                itemLists.Add(newItemList);
            }
            itemLists.ForEach((item, index) =>
            {
                if (index == 0)
                {
                   item.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 0, 0);
                }
                else
                {
                    RectTransform previousRectT = itemLists[index - 1].GetComponent<RectTransform>();
                    item.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, previousRectT.anchoredPosition.y - previousRectT.rect.height - 1, 0);
                }
            });
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
