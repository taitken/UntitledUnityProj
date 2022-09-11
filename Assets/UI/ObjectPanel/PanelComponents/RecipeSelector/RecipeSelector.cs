using System.Collections;
using System.Collections.Generic;
using TMPro;
using UI.GenericComponents;
using UI.Models;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Panel
{
    public class RecipeSelector : BaseTabContent
    {
        public RecipeSlot recipeSlotPrefab;
        private IList<RecipeSlot> recipeSlots = new List<RecipeSlot>();
        // Start is called before the first frame update
        public void Initalise(RecipePanelModel recipePanelModel)
        {
            recipePanelModel.productionBuildingModel.itemRecipes.ForEach((recipe, index) =>
            {
                RecipeSlot newSlot = Instantiate(this.recipeSlotPrefab, Vector3.zero, default(UnityEngine.Quaternion));
                this.recipeSlots.Add(newSlot);
                newSlot.transform.SetParent(this.transform);
                newSlot.GetComponent<RectTransform>().position = this.recipeSlotPrefab.GetComponent<RectTransform>().position
                    + new Vector3(0, index * (-this.recipeSlotPrefab.GetComponent<Image>().rectTransform.rect.height));
                newSlot.gameObject.SetActive(true);
                newSlot.Construct(recipe);
            });
        }
    }
}
