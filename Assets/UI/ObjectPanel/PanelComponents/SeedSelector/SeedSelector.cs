using System.Collections;
using System.Collections.Generic;
using TMPro;
using UI.GenericComponents;
using UI.Models;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Panel
{
    public class SeedSelector : BaseTabContent
    {
        public SeedSlot seedSlotPrefab;
        private IList<SeedSlot> seedSlots = new List<SeedSlot>();
        // Start is called before the first frame update
        public void Initalise(SeedSelectorPanelModel seedSelectorPanelModel)
        {
            // seedSelectorPanelModel.growerBuildingModel.itemRecipes.ForEach((recipe, index) =>
            // {
            //     SeedSlot newSlot = Instantiate(this.seedSlotPrefab, Vector3.zero, default(UnityEngine.Quaternion));
            //     this.seedSlots.Add(newSlot);
            //     newSlot.transform.SetParent(this.transform);
            //     newSlot.GetComponent<RectTransform>().position = this.seedSlotPrefab.GetComponent<RectTransform>().position
            //         + new Vector3(0, index * (-this.seedSlotPrefab.GetComponent<Image>().rectTransform.rect.height));
            //     newSlot.gameObject.SetActive(true);
            //     newSlot.Construct(recipe);
            // });
        }
    }
}
