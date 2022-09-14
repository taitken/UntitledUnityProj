using System;
using System.Collections.Generic;
using Crops.Models;
using Item.Models;
using UnityEngine;

namespace Building.Models
{
    public class GrowerBuildingModel : BuildingObjectModel
    {
        public eCropType? selectedCropType;
        public CropObjectModel cropObject;
        public GrowerBuildingModel(Vector3Int _position, eBuildingType _buildingType, BuildingStatsModel _buildStats) : base(_position, _buildingType, _buildStats)
        {

        }
    }
}

