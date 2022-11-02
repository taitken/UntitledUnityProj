using System;
using System.Collections.Generic;
using Building.Models;
using UnityEngine;

namespace UI.Models
{
    public class RecipePanelModel : BasePanelModel
    {
        public ProductionBuildingModel productionBuildingModel;
        public RecipePanelModel(long _objectID, string _title, ProductionBuildingModel _productionBuildingModel) : base(_objectID, _title, ePanelTypes.RecipeSelector)
        {
            this.productionBuildingModel = _productionBuildingModel;
        }
    }
}

