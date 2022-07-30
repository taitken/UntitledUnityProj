using System.Globalization;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Zenject;

namespace UI
{
    public class ItemList : MonoBehaviour2
    {
        private ItemMassText itemMassText;
        private ItemNameText itemNameText;
        private ItemThumbnail itemThumbnail;
        private ItemListModel itemListModel;

        [Inject]
        public void Construct(ItemListModel _itemListModel)
        {
            this.itemListModel = _itemListModel;
            this.itemMassText = this.GetComponentInChildren<ItemMassText>();
            this.itemNameText = this.GetComponentInChildren<ItemNameText>();
            this.itemThumbnail = this.GetComponentInChildren<ItemThumbnail>();
            this.SetModel(this.itemListModel);
        }
        public void SetModel(ItemListModel _itemListModel)
        {
            this.itemListModel = _itemListModel;
            this.itemNameText.SetText(this.itemListModel.itemType.ToString());
            this.itemMassText.SetText(this.itemListModel.mass.ToString() + "kg");
            this.itemThumbnail.SetImage(this.itemListModel.sprite);
        }
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public class Factory : PlaceholderFactory<ItemListModel, ItemList>
        {
            
        }
    }
}