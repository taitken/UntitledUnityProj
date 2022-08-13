using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using UI.Models;
using UI.GenericComponents;
using Building.Models;
using Item.Models;
using UnityEngine.UI;

namespace UI
{
    public class ProductionBuildingContextWindow : ContextWindow
    {
        private CWTitle cwTitle;
        private ProgressBar progressBar;
        private ProductionBuildingContextWindowModel contextWindowModel;
        private IList<ItemSlot> inputSlots = new List<ItemSlot>();
        private IList<ItemSlot> outputSlots = new List<ItemSlot>();
        private IList<(eItemType, Sprite)> itemSprites;

        public override void Construct(ContextWindowModel _contextWindowModel)
        {
            this.contextWindowModel = _contextWindowModel as ProductionBuildingContextWindowModel;
            this.cwTitle = this.GetComponentInChildren<CWTitle>();
            this.cwTitle.setText(this.contextWindowModel.title);
            this.progressBar = this.GetComponentInChildren<ProgressBar>();
        }

        public void SetItemSprites(IList<(eItemType, Sprite)> _itemSprites)
        {
            this.itemSprites = _itemSprites;
            this.ConfigureItemSlots();
        }

        // Update is called once per frame
        void Update()
        {
            if (this.contextWindowModel.productionBuildingModel.selectedItemRecipe != null)
            {
                this.progressBar.UpdatePercentage(100 *
                    (this.contextWindowModel.productionBuildingModel.selectedItemRecipe.productionPointsCurrent / this.contextWindowModel.productionBuildingModel.selectedItemRecipe.productionPointsMax));
            }
        }

        private void ConfigureItemSlots()
        {
            ItemSlot[] itemSlots = this.GetComponentsInChildren<ItemSlot>();
            if (this.contextWindowModel.productionBuildingModel.selectedItemRecipe != null)
            {
                this.contextWindowModel.productionBuildingModel.selectedItemRecipe.inputs.ForEach((input, index) =>
                {
                    ItemObjectModel itemObj = this.contextWindowModel.productionBuildingModel.buildingStorage.GetItem(input.itemType);
                    this.SetupNewItem(input, index, itemSlots[0], input.mass.ToString() + "kg",
                        itemObj != null ? itemObj.mass.ToString() + "kg" : "0kg");
                });
                this.contextWindowModel.productionBuildingModel.selectedItemRecipe.outputs.ForEach((input, index) =>
                {
                    this.SetupNewItem(input, index, itemSlots[1], input.mass.ToString() + "kg", "");
                });
            }
        }

        private void SetupNewItem(BuildingSupply input, int index, ItemSlot baseSlot, string requiredNumber, string currentNumber)
        {
            ItemSlot newItemSlot;
            if (index > 0)
            {
                newItemSlot = Instantiate(baseSlot, default(Vector3), default(Quaternion));
                newItemSlot.transform.SetParent(this.transform);
                newItemSlot.transform.position = baseSlot.transform.position + new Vector3(baseSlot.GetComponent<RectTransform>().rect.width + 5, 0);
                this.inputSlots.Add(newItemSlot);
            }
            else
            {
                newItemSlot = baseSlot;
                this.inputSlots.Add(newItemSlot);
            }
            newItemSlot.SetItemSprite(this.itemSprites.Find(sprite => { return sprite.Item1 == input.itemType; }).Item2);
            newItemSlot.SetRequiredNumber(requiredNumber);
            newItemSlot.SetCurrentNumber(currentNumber);
        }

        public class Factory : PlaceholderFactory<ContextWindowModel, ObjectContextWindow>
        {
        }
    }
}