using System;
using UnityEngine;
using Building.Models;
using System.Collections.Generic;
using Unit.Models;

namespace GameControllers.Models
{
    public class DeconstructOrderModel : UnitOrderModel
    {
        public BuildingObjectModel buildingModel;
        public DeconstructOrderModel(Vector3Int _coordinates, BuildingObjectModel _buildingModel, bool showIcon = false) : base(_coordinates, eOrderTypes.Deconstruct, false, showIcon)
        {
            this.buildingModel = _buildingModel;
        }

        public override bool CanAssignToUnit(IList<IBaseService> _services, UnitModel _unitModel)
        {
            return base.CanAssignToUnit(_services, _unitModel);
        }
    }
}

