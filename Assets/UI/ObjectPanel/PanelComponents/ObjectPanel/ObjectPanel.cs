using System.Collections;
using System.Collections.Generic;
using ObjectComponents;
using TMPro;
using UI.Models;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Panel
{
    public class ObjectPanel : BasePanel
    {
        private ObjectPanelModel objectPanelModel;
        public PanelWindow panelWindow;
        public DetailsTab detailTab;
        public StorageTab storageTab;
        public CharacterNeedsTab charNeedsTab;

        public override void Construct(BasePanelModel panelWindowModel)
        {
            this.objectPanelModel = panelWindowModel as ObjectPanelModel;
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
            if (this.detailTab.Initalise(this.objectPanelModel.objectModel)) tabs.Add((this.detailTab, "Details"));
            tabs.Add((this.detailTab, "Details2"));
            if (this.storageTab.Initalise(this.objectPanelModel.objectModel)) tabs.Add((this.storageTab, "Storage"));
            if (this.charNeedsTab.Initalise(this.objectPanelModel.objectModel)) tabs.Add((this.charNeedsTab, "Needs"));
            this.panelWindow.Initialise(tabs);
        }
    }
}
