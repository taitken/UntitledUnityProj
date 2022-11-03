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
        public IconButtonSlot seedSlotPrefab;
        public Color defaultBG;
        public Color selectedBG;
        private SeedSelectorPanelModel seedSelectorPanelModel;
        private IList<IconButtonSlot> seedSlots = new List<IconButtonSlot>();
        // Start is called before the first frame update
        public void Initalise(SeedSelectorPanelModel _seedSelectorPanelModel,
                              IItemObjectService itemService,
                              ICropService cropService)
        {
            this.seedSelectorPanelModel = _seedSelectorPanelModel;
            cropService.GetAllCropStats().ForEach((stats, index) =>
            {
                IconButtonSlot newSlot = Instantiate(this.seedSlotPrefab, Vector3.zero, default(UnityEngine.Quaternion));
                this.seedSlots.Add(newSlot);
                newSlot.transform.parent = this.transform;
                newSlot.gameObject.SetActive(true);
                newSlot.Initialise(stats.cropName, cropService.GetCropSpriteSet(stats.cropType)[4], (int)stats.cropType);
                newSlot.onButtonSelectEmitter.OnEmit(this.OnCropTypeSelect);
                Vector3 pos = UiPositioningHelper.GetIconButtonSlotPosition(index, this.seedSlotPrefab.gameObject, ITEMS_PER_ROW);
                newSlot.GetComponent<RectTransform>().localPosition = pos - new Vector3(36, -126.5f);
            });
            if (_seedSelectorPanelModel.growerBuildingModel.selectedCropType != null)
            {
                this.seedSlots.Find(slot => { return (eCropType)slot.returnEnemurator == _seedSelectorPanelModel.growerBuildingModel.selectedCropType; }).SetBackgroundColor(this.selectedBG);
            }
        }

        private void OnCropTypeSelect(int cropType)
        {
            // Select something new
            if (this.seedSelectorPanelModel.growerBuildingModel.selectedCropType != (eCropType)cropType)
            {
                this.seedSelectorPanelModel.growerBuildingModel.selectedCropType = (eCropType)cropType;
                this.seedSlots.ForEach(slot => { slot.SetBackgroundColor(this.defaultBG); });
                this.seedSlots.Find(slot => { return slot.returnEnemurator == cropType; }).SetBackgroundColor(this.selectedBG);
            }
            // Select already selected
            else if (this.seedSelectorPanelModel.growerBuildingModel.selectedCropType == (eCropType)cropType)
            {
                this.seedSelectorPanelModel.growerBuildingModel.selectedCropType = null;
                this.seedSlots.ForEach(slot => { slot.SetBackgroundColor(this.defaultBG); });
            }
            this.seedSelectorPanelModel.growerBuildingModel.NotifyModelUpdate();
        }
    }
}
