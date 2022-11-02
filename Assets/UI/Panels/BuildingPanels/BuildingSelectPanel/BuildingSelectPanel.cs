using System.Collections;
using System.Collections.Generic;
using ObjectComponents;
using TMPro;
using UI.Models;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Panel
{
    public class BuildingSelectPanel : BasePanel
    {
        private BuildingSelectorPanelModel buildingSelectPanelModel;
        public PanelWindow panelWindow;

        public override void Construct(BasePanelModel panelWindowModel)
        {
            this.buildingSelectPanelModel = panelWindowModel as BuildingSelectorPanelModel;
            TextMeshProUGUI[] texts = GetComponentsInChildren<TextMeshProUGUI>();
            texts.ForEach(text =>
            {
                if (text.tag == "UiHeader") text.SetText(panelWindowModel.title);
            });
        }

        private void ConfigureTabs()
        {

        }
    }
}
