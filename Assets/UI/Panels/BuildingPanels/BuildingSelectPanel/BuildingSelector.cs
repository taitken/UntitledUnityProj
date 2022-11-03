using System.Collections;
using System.Collections.Generic;
using Building.Models;
using Crops.Models;
using GameControllers.Models;
using GameControllers.Services;
using TMPro;
using UI.GenericComponents;
using UI.Models;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Panel
{
    public class BuildingSelector : BaseTabContent
    {
        private const int ITEMS_PER_ROW = 3;
        public IconButtonSlot buildingSlot;
        public Color defaultBG;
        public Color selectedBG;
        private BuildingSelectorPanelModel buildingSelectorPanelModel;
        private IUnitOrderService orderService;
        private IList<IconButtonSlot> seedSlots = new List<IconButtonSlot>();
        // Start is called before the first frame update
        public void Initalise(BuildingSelectorPanelModel _buildingSelectorPanelModel,
                              IUnitOrderService _orderService,
                              IBuildingService buildingService)
        {
            this.orderService = _orderService;
            this.buildingSelectorPanelModel = _buildingSelectorPanelModel;
            BuildingStatsLibrary.GetBuildingStats(this.buildingSelectorPanelModel.buildingCategory).ForEach((stats, index) =>
            {
                IconButtonSlot newSlot = this.buildingSlot.CreateNew(this.transform, stats.buildingName, buildingService.GetBuildingSprite(stats.buildingType).sprite, (int)stats.buildingType);
                this.seedSlots.Add(newSlot);
                newSlot.onButtonSelectEmitter.OnEmit(this.OnBuildingSelect);
                Vector3 pos = UiPositioningHelper.GetIconButtonSlotPosition(index, newSlot.gameObject, ITEMS_PER_ROW);
                newSlot.GetComponent<RectTransform>().localPosition = pos - new Vector3(36, -126.5f);
            });
        }

        private void OnBuildingSelect(int buildingType)
        {
            this.orderService.mouseAction.Set(new MouseActionModel(eMouseAction.Build, (eBuildingType)buildingType));
        }
    }
}
