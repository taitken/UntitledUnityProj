using System;
using System.Collections.Generic;
using Crops.Models;
using Item.Models;
using ObjectComponents;
using UnityEngine;

namespace Building.Models
{
    public class GrowerBuildingModel : BuildingObjectModel
    {
        public eCropType? selectedCropType { get; set; }
        public CropObjectModel cropObject { get; set; }
        public ObjectStorageComponent buildingStorage { get; set; }
        public GrowerBuildingModel(Vector3Int _position, eBuildingType _buildingType, BuildingStatsModel _buildStats) : base(_position, _buildingType, _buildStats)
        {
            this.buildingStorage = new ObjectStorageComponent();
            this.objectComponents.Add(this.buildingStorage);
        }
    }
}

