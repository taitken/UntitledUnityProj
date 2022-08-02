using System;
using System.Collections.Generic;
using UnityEngine;

namespace Building.Models
{
    public class BuildingObjectModel : BasePhysicalObjectModel
    {
        public eBuildingType buildingType { get; set; }
        public IList<BuildingSupply> requiredItems { get; set; }
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
        public BuildingObjectModel(Vector3Int _position, eBuildingType _buildingType, BuildingStatsModel _buildStats) : base(_position, 0)
        {
            this.buildingType = _buildingType;
            this.requiredItems = _buildStats.buildSupply;
            this.size = _buildStats.size;
        }
    }
}

