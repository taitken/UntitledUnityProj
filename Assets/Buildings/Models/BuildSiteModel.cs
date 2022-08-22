using System;
using System.Collections.Generic;
using Item.Models;
using ObjectComponents;
using UnityEngine;

namespace Building.Models
{
    public class BuildSiteModel : BaseObjectModel
    {
        public eBuildingType buildingType;
        public BuildingObjectModel buildingModel;
        public IList<ItemObjectModel> suppliedItems { get; }

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
                    if (this.suppliedItems.Filter(item => { return item.itemType == requiredItem.itemType; }).Sum(item => { return item.mass; }) < requiredItem.mass)
                    {
                        suppliedItems = false;
                    }
                });
                return suppliedItems;
            }
        }

        public BuildSiteModel(BuildingObjectModel buildingModel) : base(buildingModel.position, buildingModel.requiredItems.Map(item => { return new ItemObjectMass(item.itemType, 0); }))
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
            this.GetObjectComponent<ObjectComposition>().AddMass(itemObj.itemType, itemObj.mass);
        }
    }
}

