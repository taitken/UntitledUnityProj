using System;
using System.Collections;
using System.Collections.Generic;
using Building.Models;
using TMPro;
using UI.GenericComponents;
using UI.Models;
using UnityEngine;
using UnityEngine.UI;
using UtilityClasses;

namespace UI.Panel
{
    public class SeedSlot : MonoBehaviour2
    {
        public Color originalColour;
        public GameObject background;
        private AllocatedItemRecipe allocatedItemRecipe;
        public TextMeshProUGUI recipeCounter;
        public TextMeshProUGUI itemName;
        // Start is called before the first frame update

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

        public void OnMouseClick()
        {

        }

        public override void OnMouseEnter()
        {
            MouseIconSingleton.SetCursorTexure(GameControllers.Models.eMouseAction.Pointer);
            this.originalColour = this.background.GetComponent<Image>().color;
            this.background.GetComponent<Image>().color = this.originalColour;
            this.background.GetComponent<Image>().color = new Color(0.9765f, 0.9765f, 0.9765f);
        }
        public override void OnMouseExit()
        {
            MouseIconSingleton.SetCursorTexure(GameControllers.Models.eMouseAction.None);
            this.background.GetComponent<Image>().color = this.originalColour;
        }


    }
}