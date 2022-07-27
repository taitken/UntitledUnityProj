using System;
using UnityEngine;
using Building.Models;
using System.Collections.Generic;
using Unit.Models;
using GameControllers.Services;
using Item.Models;

namespace GameControllers.Models
{
    public class StoreOrderModel : UnitOrderModel
    {
        public ItemObjectModel itemModel {get;set;}
        public StoreOrderModel(Vector3Int _coordinates,  ItemObjectModel _itemModel, bool showIcon = true) : base(_coordinates, eOrderTypes.Store, showIcon)
        {
            this.itemModel = _itemModel;
        }

        public override bool CanAssignToUnit(IList<IBaseService> _services, UnitModel _unitModel)
        {
            IBuildingService buildService = this.GetService<IBuildingService>(_services);
            if (buildService == null) return false;
            return  base.CanAssignToUnit(_services, _unitModel) &&  buildService.IsStorageAvailable();
        }
    }
}

