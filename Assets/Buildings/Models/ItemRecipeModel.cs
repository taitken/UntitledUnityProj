using System;
using System.Collections.Generic;

namespace Building.Models
{
    public class ItemRecipeModel : BaseModel
    {
        public string recipeName { get; set; }
        public IList<BuildingSupply> inputs { get; set; }
        public IList<BuildingSupply> outputs { get; set; }
        public float productionPointsMax { get; set; }
        public float productionPointsCurrent { get; set; }
        public ItemRecipeModel(string _recipeName, IList<BuildingSupply> _inputs, IList<BuildingSupply> _outputs, float _productionPointsMax) : base()
        {
            this.recipeName = _recipeName;
            this.inputs = _inputs;
            this.outputs = _outputs;
            this.productionPointsMax = _productionPointsMax;
            this.productionPointsCurrent = 0;
        }
    }
}