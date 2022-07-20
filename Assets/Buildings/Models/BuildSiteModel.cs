using System;
using System.Collections.Generic;
using Item.Models;
using UnityEngine;

namespace Building.Models
{
    public class BuildSiteModel : BasePhysicalObjectModel
    {
        public eBuildingType buildingType;
        public BuildingObjectModel buildingModel;
        public IList<ItemObjectModel> suppliedItems { get; set; }

        public decimal supplyCurrent
        {
            get
            {
                decimal supplyCurrent = 0;
                this.suppliedItems.ForEach(item => { supplyCurrent += item.mass; });
                return supplyCurrent;
            }
        }
        public bool isFullySupplied
        {
            get
            {
                bool suppliedItems = true;
                this.buildingModel.requiredItems.ForEach(requiredItem =>
                {
                    if (this.suppliedItems.Filter(item => { return item.itemType == requiredItem.itemType; }).AddNumbers(item => { return item.mass; }) < requiredItem.mass)
                    {
                        suppliedItems = false;
                    }
                });
                return suppliedItems;
            }
        }

        public BuildSiteModel(BuildingObjectModel buildingModel) : base(buildingModel.position, 0)
        {
            this.buildingType = buildingModel.buildingType;
            this.buildingModel = buildingModel;
            this.suppliedItems = new List<ItemObjectModel>();
        }

        public void SupplyItem(ItemObjectModel itemObj)
        {
            itemObj.itemState = ItemObjectModel.eItemState.InSupply;
            itemObj.position = this.position;
            this.suppliedItems.Add(itemObj);
        }
    }
}

