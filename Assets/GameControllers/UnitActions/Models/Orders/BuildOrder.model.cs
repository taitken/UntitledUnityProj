using System;
using UnityEngine;
using Building.Models;
using System.Collections.Generic;
using Unit.Models;

namespace GameControllers.Models
{
    public class BuildOrderModel : UnitOrderModel
    {
        public eBuildingType buildingType;
        public BuildOrderModel(Vector3Int _coordinates, eBuildingType _buildingType, bool showIcon = false) : base(_coordinates, eOrderTypes.Build, showIcon)
        {
            this.buildingType = _buildingType;
        }

        public override bool CanAssignToUnit(IList<IBaseService> _services, UnitModel _unitModel)
        {
            return base.CanAssignToUnit(_services, _unitModel);
        }
    }
}

