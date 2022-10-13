using System;
using UnityEngine;
using Building.Models;
using System.Collections.Generic;
using Unit.Models;
using Environment.Models;

namespace GameControllers.Models
{
    public class DigOrderModel : UnitOrderModel
    {
        public MineableObjectModel targetToMine;
        public DigOrderModel(MineableObjectModel _targetToMine, bool showIcon = true) : base(_targetToMine.position, eOrderTypes.Dig, showIcon)
        {
            this.targetToMine = _targetToMine;
        }

        public override bool CanAssignToUnit(IList<IBaseService> _services, UnitModel _unitModel)
        {
            return base.CanAssignToUnit(_services, _unitModel);
        }
    }
}

