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
            if (this.ConfigureDetailsTab()) tabs.Add((this.detailTab, "Details"));
            tabs.Add((this.detailTab, "Details2"));
            if (this.ConfigureStorageTab()) tabs.Add((this.storageTab, "Storage"));
            this.panelWindow.Initialise(tabs);
        }

        private bool ConfigureDetailsTab()
        {
            ObjectComposition objectComposition = this.objectPanelModel.objectModel.GetObjectComponent<ObjectComposition>();
            if (objectComposition != null)
            {
                this.detailTab.Initalise(objectComposition);
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool ConfigureStorageTab()
        {
            ObjectStorage storageComponent = this.objectPanelModel.objectModel.GetObjectComponent<ObjectStorage>();
            if (storageComponent != null)
            {
                this.storageTab.Initalise(storageComponent);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
