using System.Collections;
using System.Collections.Generic;
using TMPro;
using UI.GenericComponents;
using UI.Models;

namespace UI.Panel
{
    public class RecipeSelector : BasePanel
    {
        // Start is called before the first frame update
        public override void Construct(BasePanelModel panelWindowModel)
        {
            RecipePanelModel recipePanelModel = panelWindowModel as RecipePanelModel;
            TextMeshProUGUI[] texts = GetComponentsInChildren<TextMeshProUGUI>();
            texts.ForEach(text =>
            {
                if (text.tag == "UiHeader") text.SetText(recipePanelModel.title);
            });
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
