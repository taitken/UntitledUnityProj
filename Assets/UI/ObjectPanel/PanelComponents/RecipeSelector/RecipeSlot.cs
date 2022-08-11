using System;
using System.Collections;
using System.Collections.Generic;
using Building.Models;
using TMPro;
using UI.GenericComponents;
using UI.Models;
using UnityEngine;

namespace UI.Panel
{
    public class RecipeSlot : MonoBehaviour2
    {
        private AllocatedItemRecipe allocatedItemRecipe;
        public TriangleButton triangleButtonLeft;
        public TriangleButton triangleButtonRight;
        public TextMeshProUGUI recipeCounter;
        public TextMeshProUGUI itemName;
        // Start is called before the first frame update
        void Start()
        {
            this.triangleButtonLeft.GetComponent<TriangleButton>().onClickEmitter.OnEmit(() =>
            {
                this.IncrementRecipeCounterVal(-1);
            });
            this.triangleButtonRight.GetComponent<TriangleButton>().onClickEmitter.OnEmit(() =>
            {
                this.IncrementRecipeCounterVal(1);
            });
        }

        public void Construct(AllocatedItemRecipe _allocatedItemRecipe)
        {
            this.allocatedItemRecipe = _allocatedItemRecipe;
            this.itemName.SetText(this.allocatedItemRecipe.recipe.recipeName);
            this.recipeCounter.SetText(this.allocatedItemRecipe.counter.ToString());
        }

        private void IncrementRecipeCounterVal(int increment)
        {
            int number = Math.Max(0, int.Parse(this.recipeCounter.text) + increment);
            this.recipeCounter.SetText(number.ToString());
            this.allocatedItemRecipe.counter = number;
        }

        public void OnHoverButton()
        {

        }


    }
}