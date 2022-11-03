using System.Collections;
using System.Collections.Generic;
using GameControllers.Services;
using ObjectComponents;
using TMPro;
using UI.Models;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.Panel
{
    public class BuildingSelectPanel : BasePanel
    {
        private BuildingSelectorPanelModel buildingSelectPanelModel;
        public BuildingSelector buildingSelector;

        [Inject]
        public override void Construct(BasePanelModel panelWindowModel)
        {
            this.buildingSelectPanelModel = panelWindowModel as BuildingSelectorPanelModel;
            TextMeshProUGUI[] texts = GetComponentsInChildren<TextMeshProUGUI>();
            texts.ForEach(text =>
            {
                if (text.tag == "UiHeader") text.SetText(panelWindowModel.title);
            });
            buildingSelector.Initalise(this.buildingSelectPanelModel, this.GetService<IUnitOrderService>(), this.GetService<IBuildingService>());
        }

        private void ConfigureTabs()
        {

        }
    }
}
