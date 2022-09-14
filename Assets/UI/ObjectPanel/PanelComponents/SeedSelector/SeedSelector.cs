using System.Collections;
using System.Collections.Generic;
using Crops.Models;
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
        public Color defaultBG;
        public Color selectedBG;
        private SeedSelectorPanelModel seedSelectorPanelModel;
        private IList<SeedSlot> seedSlots = new List<SeedSlot>();
        // Start is called before the first frame update
        public void Initalise(SeedSelectorPanelModel _seedSelectorPanelModel,
                              IItemObjectService itemService,
                              ICropService cropService)
        {
            this.seedSelectorPanelModel = _seedSelectorPanelModel;
            cropService.GetAllCropStats().ForEach((stats, index) =>
            {
                SeedSlot newSlot = Instantiate(this.seedSlotPrefab, Vector3.zero, default(UnityEngine.Quaternion));
                this.seedSlots.Add(newSlot);
                newSlot.transform.parent = this.transform;
                newSlot.gameObject.SetActive(true);
                newSlot.Initialise(stats.cropName, cropService.GetCropSpriteSet(stats.cropType)[4], stats.cropType);
                newSlot.onCropTypeSelectEmitter.OnEmit(this.OnCropTypeSelect);
                Vector3 pos = this.GetSeedSlotPosition(index);
                newSlot.GetComponent<RectTransform>().localPosition = this.GetSeedSlotPosition(index) - new Vector3(36, -126.5f);
            });
            if (_seedSelectorPanelModel.growerBuildingModel.selectedCropType != null)
            {
                this.seedSlots.Find(slot => { return slot.cropType == _seedSelectorPanelModel.growerBuildingModel.selectedCropType; }).SetBackgroundColor(this.selectedBG);
            }
        }

        private void OnCropTypeSelect(eCropType? cropType)
        {
            // Select something new
            if (this.seedSelectorPanelModel.growerBuildingModel.selectedCropType != cropType)
            {
                this.seedSelectorPanelModel.growerBuildingModel.selectedCropType = cropType;
                this.seedSlots.ForEach(slot => { slot.SetBackgroundColor(this.defaultBG); });
                this.seedSlots.Find(slot => { return slot.cropType == cropType; }).SetBackgroundColor(this.selectedBG);
            }
            // Select already selected
            else if (this.seedSelectorPanelModel.growerBuildingModel.selectedCropType == cropType)
            {
                this.seedSelectorPanelModel.growerBuildingModel.selectedCropType = null;
                this.seedSlots.ForEach(slot => { slot.SetBackgroundColor(this.defaultBG); });
            }
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
