using System.Collections;
using System.Collections.Generic;
using GameControllers.Services;
using TMPro;
using UI.GenericComponents;
using UI.Models;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Panel
{
    public class SeedSelector : BaseTabContent
    {
        private const int ITEMS_PER_ROW = 3;
        public SeedSlot seedSlotPrefab;
        private IList<SeedSlot> seedSlots = new List<SeedSlot>();
        // Start is called before the first frame update
        public void Initalise(SeedSelectorPanelModel seedSelectorPanelModel,
                              IItemObjectService itemService,
                              ICropService cropService)
        {
            cropService.GetAllCropStats().ForEach((stats, index) =>
            {
                SeedSlot newSlot = Instantiate(this.seedSlotPrefab, Vector3.zero, default(UnityEngine.Quaternion));
                this.seedSlots.Add(newSlot);
                newSlot.transform.parent = this.transform;
                newSlot.gameObject.SetActive(true);
                newSlot.Initialise(stats.cropName, cropService.GetCropSpriteSet(stats.cropType)[4]);
                Vector3 pos = this.GetSeedSlotPosition(index);
                newSlot.GetComponent<RectTransform>().localPosition = this.GetSeedSlotPosition(index) - new Vector3(36, -126.5f);
            });
        }

        private Vector3 GetSeedSlotPosition(int index)
        {
            float seedSlotWidth = this.seedSlotPrefab.GetComponent<RectTransform>().rect.width;
            float seedSlotHeight = -this.seedSlotPrefab.GetComponent<RectTransform>().rect.height;
            int row = (index) / ITEMS_PER_ROW;
            int column = ((index) % ITEMS_PER_ROW);
            return new Vector3((-seedSlotWidth / 2) + (seedSlotWidth * column), (seedSlotHeight / 2) + (seedSlotHeight * row), 0);
        }
    }
}
