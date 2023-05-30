using System.Collections;
using System.Collections.Generic;
using ObjectComponents;
using TMPro;
using UI.Models;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Panel
{
    public class RoomPanel : BasePanel
    {
        private RoomPanelModel roomPanelModel;
        public PanelWindow panelWindow;
        public RoomDetailsTab roomDetailsTab;

        public override void Construct(BasePanelModel panelWindowModel)
        {
            this.roomPanelModel = panelWindowModel as RoomPanelModel;
            TextMeshProUGUI[] texts = GetComponentsInChildren<TextMeshProUGUI>();
            texts.ForEach(text =>
            {
                if (text.tag == "UiHeader") text.SetText(panelWindowModel.title);
            });
            this.ConfigureTabs();
        }

        private void ConfigureTabs()
        {
            IList<(BaseTabContent, string)> tabs = new List<(BaseTabContent, string)>();
            if (this.roomDetailsTab.Initalise(this.roomPanelModel.roomModel)) tabs.Add((this.roomDetailsTab, "Bio"));
            this.panelWindow.Initialise(tabs);
        }
    }
}
