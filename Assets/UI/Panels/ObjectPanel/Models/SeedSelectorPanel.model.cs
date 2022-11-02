using System;
using System.Collections.Generic;
using Building.Models;
using UnityEngine;

namespace UI.Models
{
    public class SeedSelectorPanelModel : BasePanelModel
    {
        public GrowerBuildingModel growerBuildingModel;
        public SeedSelectorPanelModel(long _objectID, string _title, GrowerBuildingModel _growerBuildingModel) : base(_objectID, _title, ePanelTypes.SeedSelector)
        {
            this.growerBuildingModel = _growerBuildingModel;
        }
    }
}

