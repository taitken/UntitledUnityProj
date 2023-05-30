using Building.Models;
using UI.Services;
using GameControllers.Services;
using Item.Models;
using System.Collections.Generic;
using UI.Models;
using Room.Models;

namespace Building
{
    public class FloorTileObject : BuildingObject
    {
        public FloorTileModel floorTileModel;
        protected override void OnCreation()
        {
            this.floorTileModel = this.buildingObjectModel as FloorTileModel;
        }

        public override void OnSelect()
        {
            RoomModel room = this.roomService.GetRoom(this.floorTileModel);
            if (room != null)
            {
                IList<BasePanelModel> panels = new List<BasePanelModel>();
                panels.Add(new RoomPanelModel(this.buildingObjectModel.ID, this.buildingObjectModel.buildingType.ToString(), room));
                this.uiPanelService.selectedObjectPanels.Set(panels);
            }
        }
    }
}