using System;
using System.Collections.Generic;
using Item.Models;
using UnityEngine;

namespace Building.Models
{
    public class BuildSiteModel : BasePhysicalObjectModel
    {
        public BuildSiteModel(Vector3Int _position, eBuildingType _buildingType) : base(_position, 0)
        {
            this.buildingType = _buildingType;
            this.suppliedItems = new List<ItemObjectModel>();
        }
        public decimal supplyCurrent
        {
            get
            {
                decimal supplyCurrent = 0;
                this.suppliedItems.ForEach(item => { supplyCurrent += item.mass; });
                return supplyCurrent;
            }
        }
        public eBuildingType buildingType;
        public IList<ItemObjectModel> suppliedItems { get; set; }
        public void SupplyItem(ItemObjectModel itemObj)
        {
            this.suppliedItems.Add(itemObj);
        }
    }
}

