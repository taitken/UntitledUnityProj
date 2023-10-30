using System;
using UnityEngine;
using Building.Models;
using System.Collections.Generic;
using Unit.Models;
using Environment.Models;

namespace GameControllers.Models
{
    public class WanderOrderModel : UnitOrderModel
    {
        public static float WANDER_SPEED = 0.2f;
        public WanderOrderModel(Vector3Int _endPos, bool showIcon = true) : base(_endPos, eOrderTypes.Wander, true, showIcon)
        {
            
        }

        public override bool CanAssignToUnit(IList<IBaseService> _services, UnitModel _unitModel)
        {
            return base.CanAssignToUnit(_services, _unitModel);
        }
    }
}

