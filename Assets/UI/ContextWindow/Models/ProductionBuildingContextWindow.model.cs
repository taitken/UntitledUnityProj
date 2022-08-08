using System;
using System.Collections.Generic;
using Building.Models;
using UnityEngine;

namespace UI.Models
{
    public class ProductionBuildingContextWindowModel : ContextWindowModel
    {
        public ProductionBuildingModel productionBuildingModel { get; set; }
        public ProductionBuildingContextWindowModel(long _objectID, string _title, ProductionBuildingModel _productionBuildingModel) : base(_objectID, _title)
        {
            this.contextType = eContextTypes.ProductionBuilding;
            this.productionBuildingModel = _productionBuildingModel;
        }
    }
}

