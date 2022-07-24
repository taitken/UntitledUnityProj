using System;
using UnityEngine;
using Building.Models;
using System.Collections.Generic;
using Unit.Models;

namespace GameControllers.Models
{
    public class DigOrderModel : UnitOrderModel
    {
        public DigOrderModel(Vector3Int _coordinates, bool showIcon = true) : base(_coordinates, eOrderTypes.Dig, showIcon)
        {

        }

        public override bool CanAssignToUnit(IList<IBaseService> _services, UnitModel _unitModel)
        {
            return base.CanAssignToUnit(_services, _unitModel);
        }
    }
}

