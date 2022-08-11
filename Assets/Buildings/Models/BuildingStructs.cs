using System;
using System.Collections.Generic;
using GameControllers.Models;
using Item.Models;

namespace Building.Models
{
    public class AllocatedItemRecipe
    {
        public AllocatedItemRecipe(int _counter, ItemRecipeModel _recipe)
        {
            this.counter = _counter;
            this.recipe = _recipe;
        }
        public int counter { get; set; }
        public ItemRecipeModel recipe { get; set; }
    }


    public class ProductionSupplyOrder
    {
        public ProductionSupplyOrder(ProductionSupplyOrderModel _buildSupplyOrder, BuildingSupply _buildSupply )
        {
            currentBuildSupplyModel = _buildSupplyOrder;
            this.input = _buildSupply;
        }
        public ProductionSupplyOrderModel currentBuildSupplyModel { get; set; }
        public BuildingSupply input {get;set;}
    }
}