using System;
using Building.Models;
using UnityEngine;

namespace GameControllers.Models
{
    public class MouseActionModel : BaseModel
    {
        public MouseActionModel(eMouseAction _mouseType, eBuildingType _buildingType = eBuildingType.none) : base()
        {
            this.mouseType = _mouseType;
            this.buildingType = _buildingType;
        }
        public eMouseAction mouseType { get; set; }
        public eBuildingType buildingType { get; set; }
    }
}

