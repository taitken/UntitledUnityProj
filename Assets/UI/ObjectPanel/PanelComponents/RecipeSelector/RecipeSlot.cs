using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UI.GenericComponents;
using UnityEngine;

namespace UI.Panel
{
    public class RecipeSlot : MonoBehaviour
    {

        public TriangleButton triangleButtonLeft;
        public TriangleButton triangleButtonRight;
        public TextMeshProUGUI recipeCounter;
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

        private void IncrementRecipeCounterVal(int increment)
        {
            int number = Math.Max(0, int.Parse(this.recipeCounter.text) + increment);
            this.recipeCounter.SetText(number.ToString());
        }

        public void OnHoverButton()
        {

        }


    }
}