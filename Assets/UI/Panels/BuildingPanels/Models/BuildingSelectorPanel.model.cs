using System;
using System.Collections.Generic;
using Building.Models;
using UnityEngine;

namespace UI.Models
{
    public class BuildingSelectorPanelModel : BasePanelModel
    {
        eBuildingCategory buildingCategory;
        public BuildingSelectorPanelModel(string _title, eBuildingCategory _buildingCategory) : base(0, _title, ePanelTypes.BuildingSelector)
        {
            this.title = _title;
            this.buildingCategory = _buildingCategory;
        }
    }
}

