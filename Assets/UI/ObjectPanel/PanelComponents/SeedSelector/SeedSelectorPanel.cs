using System.Collections;
using System.Collections.Generic;
using TMPro;
using UI.GenericComponents;
using UI.Models;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Panel
{
    public class SeedSelectorPanel : BasePanel
    {
        public SeedSelector seedSelector;
        // Start is called before the first frame update
        public override void Construct(BasePanelModel panelWindowModel)
        {
            SeedSelectorPanelModel recipePanelModel = panelWindowModel as SeedSelectorPanelModel;
            TextMeshProUGUI[] texts = GetComponentsInChildren<TextMeshProUGUI>();
            texts.ForEach(text =>
            {
                if (text.tag == "UiHeader") text.SetText(recipePanelModel.title);
            });
            this.seedSelector.Initalise(recipePanelModel);
        }
    }
}
