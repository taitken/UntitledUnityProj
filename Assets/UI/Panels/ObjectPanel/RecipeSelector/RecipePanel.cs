using System.Collections;
using System.Collections.Generic;
using TMPro;
using UI.GenericComponents;
using UI.Models;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Panel
{
    public class RecipePanel : BasePanel
    {
        public RecipeSelector recipeSelector;
        // Start is called before the first frame update
        public override void Construct(BasePanelModel panelWindowModel)
        {
            RecipePanelModel recipePanelModel = panelWindowModel as RecipePanelModel;
            TextMeshProUGUI[] texts = GetComponentsInChildren<TextMeshProUGUI>();
            texts.ForEach(text =>
            {
                if (text.tag == "UiHeader") text.SetText(recipePanelModel.title);
            });
            this.recipeSelector.Initalise(recipePanelModel);
        }
    }
}
