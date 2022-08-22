using System;
using System.Collections.Generic;
using Item.Models;
using UnityEngine;

namespace Building.Models
{
    public class BuildingObjectModel : BaseObjectModel
    {
        public eBuildingType buildingType { get; set; }
        public IList<ItemObjectMass> requiredItems { get; set; }
        public Vector2 size { get; set; }

        public IList<Vector3Int> positions
        {
            get
            {
                IList<Vector3Int> positions = new List<Vector3Int>();
                for (int x = 0; x < this.size.x; x++)
                {
                    for (int y = 0; y < this.size.y; y++)
                    {
                        positions.Add(new Vector3Int(this.position.x + x, this.position.y + y));
                    }
                }
                return positions;
            }
        }
        public BuildingObjectModel(Vector3Int _position, eBuildingType _buildingType, BuildingStatsModel _buildStats) : base(_position, _buildStats.buildSupply)
        {
            this.buildingType = _buildingType;
            this.requiredItems = _buildStats.buildSupply;
            this.size = _buildStats.size;
        }
    }
}

