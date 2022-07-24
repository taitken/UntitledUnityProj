using System;
using UnityEngine;
using Building.Models;
using System.Collections.Generic;
using Unit.Models;
using GameControllers.Services;

namespace GameControllers.Models
{
    public class StoreOrder : UnitOrderModel
    {
        public StoreOrder(Vector3Int _coordinates, bool showIcon = true) : base(_coordinates, eOrderTypes.Store, showIcon)
        {

        }

        public override bool CanAssignToUnit(IList<IBaseService> _services, UnitModel _unitModel)
        {
            IBuildingService buildService = this.GetService<IBuildingService>(_services);
            if (buildService == null) return false;
            return  base.CanAssignToUnit(_services, _unitModel) &&  buildService.IsStorageAvailable();
        }
    }
}

