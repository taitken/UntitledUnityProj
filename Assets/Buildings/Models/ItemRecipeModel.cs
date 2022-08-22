using System;
using System.Collections.Generic;
using Item.Models;

namespace Building.Models
{
    public class ItemRecipeModel : BaseModel
    {
        public string recipeName { get; set; }
        public IList<ItemObjectMass> inputs { get; set; }
        public IList<ItemObjectMass> outputs { get; set; }
        public float productionPointsMax { get; set; }
        public float productionPointsCurrent { get; set; }
        public ItemRecipeModel(string _recipeName, IList<ItemObjectMass> _inputs, IList<ItemObjectMass> _outputs, float _productionPointsMax) : base()
        {
            this.recipeName = _recipeName;
            this.inputs = _inputs;
            this.outputs = _outputs;
            this.productionPointsMax = _productionPointsMax;
            this.productionPointsCurrent = 0;
        }
    }
}