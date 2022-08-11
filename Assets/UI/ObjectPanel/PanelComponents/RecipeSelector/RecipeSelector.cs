using System.Collections;
using System.Collections.Generic;
using TMPro;
using UI.GenericComponents;
using UI.Models;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Panel
{
    public class RecipeSelector : BasePanel
    {
        public RecipeSlot recipeSlotPrefab;
        private IList<RecipeSlot> recipeSlot = new List<RecipeSlot>();
        // Start is called before the first frame update
        public override void Construct(BasePanelModel panelWindowModel)
        {
            RecipePanelModel recipePanelModel = panelWindowModel as RecipePanelModel;
            TextMeshProUGUI[] texts = GetComponentsInChildren<TextMeshProUGUI>();
            texts.ForEach(text =>
            {
                if (text.tag == "UiHeader") text.SetText(recipePanelModel.title);
            });
            recipePanelModel.productionBuildingModel.itemRecipes.ForEach((recipe, index) =>
            {
                RecipeSlot newSlot = Instantiate(this.recipeSlotPrefab, Vector3.zero, default(UnityEngine.Quaternion));
                recipeSlot.Add(newSlot);
                newSlot.transform.SetParent(this.transform);
                newSlot.GetComponent<RectTransform>().position = this.recipeSlotPrefab.GetComponent<RectTransform>().position
                    + new Vector3(0, index * (-this.recipeSlotPrefab.GetComponent<Image>().rectTransform.rect.height));
                newSlot.gameObject.SetActive(true);
                newSlot.Construct(recipe);
            });
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
