using System;
using System.Collections.Generic;
using Crops.Models;
using Item.Models;
using ObjectComponents;
using UnityEngine;

namespace Building.Models
{
    [ObjectComponentAttribute(typeof(ObjectStorageComponent))]
    public class GrowerBuildingModel : BuildingObjectModel
    {
        public eCropType? selectedCropType { get; set; }
        public CropObjectModel cropObject { get; set; }
        public ObjectStorageComponent buildingStorage { get { return this.GetObjectComponent<ObjectStorageComponent>(); } }
        public GrowerBuildingModel(Vector3Int _position, eBuildingType _buildingType, BuildingStatsModel _buildStats) : base(_position, _buildingType, _buildStats)
        {
            
        }
    }
}

