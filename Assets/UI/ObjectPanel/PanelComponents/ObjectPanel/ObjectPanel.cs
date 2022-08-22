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
        public PanelTabSection panelTabSection;
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
            IList<(ePanelTabTypes, string)> tabs = new List<(ePanelTabTypes, string)>();
            tabs.Add((ePanelTabTypes.Details, "Details"));
            tabs.Add((ePanelTabTypes.Details, "Details2"));
            if (this.ConfigureStorageTab()) tabs.Add((ePanelTabTypes.Storage, "Storage"));
            this.panelTabSection.Initalise(this.GetComponent<Image>().rectTransform.sizeDelta.x - 2, tabs);
            this.panelTabSection.OnTabSelect.OnEmit(this.OnTabSelect);
            this.OnTabSelect(ePanelTabTypes.Details);
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

        private void OnTabSelect(ePanelTabTypes tabSelected)
        {
            this.detailTab.gameObject.SetActive(false);
            this.storageTab.gameObject.SetActive(false);
            switch (tabSelected)
            {
                case ePanelTabTypes.Details:
                    this.detailTab.gameObject.SetActive(true);
                    break;
                case ePanelTabTypes.Storage:
                    this.storageTab.gameObject.SetActive(true);
                    break;
            }
        }
    }
}
