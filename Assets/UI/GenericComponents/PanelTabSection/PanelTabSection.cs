using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UI.Models;
using UnityEngine;
using UnityEngine.UI;
using UtilityClasses;

namespace UI.Panel
{
    public class PanelTabSection : MonoBehaviour2
    {
        public Image border;
        public EventEmitter<ePanelTabTypes> OnTabSelect = new EventEmitter<ePanelTabTypes>();
        public PanelTabBackground background;
        public TextMeshProUGUI label;
        private Vector2 defaultBackgroundSize;
        private IList<TabSectionModel> sections { get; set; } = new List<TabSectionModel>();
        // Start is called before the first frame update

        public void Initalise(float width, IList<(ePanelTabTypes, string)> sectionTypes)
        {
            float newWidth = width / sectionTypes.Count;

            this.background = Instantiate(this.background);
            this.background.transform.SetParent(this.transform);
            this.background.transform.SetSiblingIndex(1);
            TabSectionModel tabSectionPrefab = new TabSectionModel(this.border, this.background, this.label);
            this.sections.Add(tabSectionPrefab);

            this.defaultBackgroundSize = new Vector2(newWidth - 2, this.background.GetComponent<Image>().rectTransform.sizeDelta.y);
            this.label.SetText(sectionTypes[0].Item2);
            for (int i = 0; i < sectionTypes.Count - 1; i++)
            {
                this.sections.Add(tabSectionPrefab.Copy(this.transform, sectionTypes[i + 1].Item2));
            }
            this.sections.ForEach((section, index) =>
            {
                section.background.onClickEmitter.OnEmit(tab =>
                {
                    this.sections.ForEach(existingRT => { existingRT.background.GetComponent<Image>().rectTransform.sizeDelta = this.defaultBackgroundSize; });
                    tab.GetComponent<Image>().rectTransform.sizeDelta = new Vector2(tab.GetComponent<Image>().rectTransform.sizeDelta.x, tab.GetComponent<Image>().rectTransform.sizeDelta.y + 2);
                    this.OnTabSelect.Emit(sectionTypes[index].Item1);
                });
                RectTransform borderRT = section.border.GetComponent<RectTransform>();
                RectTransform backgroundRT = section.background.GetComponent<RectTransform>();
                RectTransform labelRT = section.label.GetComponent<RectTransform>();
                borderRT.sizeDelta = new Vector2(newWidth, borderRT.sizeDelta.y);
                backgroundRT.sizeDelta = this.defaultBackgroundSize;
                labelRT.sizeDelta = new Vector2(newWidth, labelRT.sizeDelta.y);
                if (index == 0)
                {
                    borderRT.position = new Vector2(borderRT.position.x - (width / 2) + (newWidth / 2), borderRT.position.y);
                    backgroundRT.position = borderRT.position;
                    labelRT.position = new Vector2(labelRT.position.x - (width / 2) + (newWidth / 2) + 7, labelRT.position.y);
                }
                else
                {
                    Vector3 initialPos = tabSectionPrefab.border.GetComponent<RectTransform>().position;
                    backgroundRT.position = new Vector2(initialPos.x + (newWidth * index), initialPos.y);
                    borderRT.position = new Vector2(initialPos.x + (newWidth * index), initialPos.y);
                    labelRT.position = new Vector2(initialPos.x + ((newWidth) * index) + 7, initialPos.y);
                }
            });
            this.sections[0].background.onClickEmitter.Emit(this.sections[0].background);
        }

        private class TabSectionModel
        {
            public Image border;
            public PanelTabBackground background;
            public TextMeshProUGUI label;

            public TabSectionModel(Image _border, PanelTabBackground _background, TextMeshProUGUI _label)
            {
                this.border = _border;
                this.background = _background;
                this.label = _label;
            }

            public TabSectionModel Copy(Transform _parentTransform, string title)
            {
                Image border = Instantiate(this.border);
                PanelTabBackground background = Instantiate(this.background);
                TextMeshProUGUI label = Instantiate(this.label);
                border.transform.SetParent(_parentTransform);
                background.transform.SetParent(_parentTransform);
                label.transform.SetParent(_parentTransform);
                label.SetText(title);
                return new TabSectionModel(border, background, label);
            }
        }
    }


}
