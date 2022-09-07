using System.Collections;
using System.Collections.Generic;
using TMPro;
using UI.Models;
using UI.Panel;
using UnityEngine;
using UnityEngine.UI;

public class PanelWindow : MonoBehaviour2
{
    public PanelTabSection panelTabSection;
    private IList<(BaseTabContent, string)> contentTabs;

    public void Initialise(IList<(BaseTabContent, string)> tabs)
    {
        this.ConfigTabs(tabs);
    }

    private void ConfigTabs(IList<(BaseTabContent, string)> tabs)
    {
        this.contentTabs = tabs;
        this.panelTabSection.Initalise(this.GetComponent<Image>().rectTransform.sizeDelta.x - 2, tabs);
        this.panelTabSection.OnTabSelect.OnEmit(this.OnTabSelect);
        this.OnTabSelect(tabs[0].Item1);
    }

    private void OnTabSelect(BaseTabContent tabSelected)
    {
        this.contentTabs.ForEach(tab =>{ tab.Item1.gameObject.SetActive(false);});
        tabSelected.gameObject.SetActive(true);
    }

}
