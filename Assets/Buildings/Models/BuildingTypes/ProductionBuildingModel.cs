using System;
using System.Collections.Generic;
using Item.Models;
using UnityEngine;

namespace Building.Models
{
    public class ProductionBuildingModel : BuildingObjectModel
    {
        public IList<BuildingSupply> productionSupplyMax { get; set; }
        public IList<ItemObjectModel> productionSupplyCurrent { get; set; }
        public int productionPointsMax { get; set; }
        public int productionPointsCurrent { get; set; }
        public IList<BuildingSupply> inputs { get; set; }
        public IList<BuildingSupply> outputs { get; set; }
        public ProductionBuildingModel(Vector3Int _position, eBuildingType _buildingType, BuildingStatsModel _buildStats)
            : base(_position, _buildingType, _buildStats)
        {
            this.productionSupplyCurrent = new List<ItemObjectModel>();
            this.productionSupplyMax = _buildStats.productionSupply;
            this.productionPointsMax = _buildStats.productionPointsMax;
            this.productionPointsCurrent = 0;
            this.inputs = _buildStats.inputs;
            this.outputs = _buildStats.outputs;
        }
    }
}

