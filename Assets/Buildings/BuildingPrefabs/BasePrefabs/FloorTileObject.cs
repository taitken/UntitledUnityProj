using Building.Models;
using UI.Services;
using GameControllers.Services;
using Item.Models;
using System.Collections.Generic;
using UI.Models;
using Room.Models;
using UtilityClasses;
using UnityEngine;

namespace Building
{
    public class FloorTileObject : BuildingObject
    {
        public FloorTileModel floorTileModel;
        private long selectedRoomID;
        protected override void OnCreation()
        {
            this.floorTileModel = this.buildingObjectModel as FloorTileModel;
        }

        public override void OnSelect()
        {
            RoomModel room = this.roomService.GetRoom(this.floorTileModel);
            if (room != null && (this.roomService.selectedRoomObservable.Get() == null || this.roomService.selectedRoomObservable.Get().ID != room.ID))
            {
                this.roomService.SelectRoom(room);
                if (room != null)
                {
                    this.selectedRoomID = room.ID;
                    IList<BasePanelModel> panels = new List<BasePanelModel>();
                    panels.Add(new RoomPanelModel(this.buildingObjectModel.ID, this.buildingObjectModel.buildingType.ToString(), room));
                    this.uiPanelService.selectedObjectPanels.Set(panels);

                    // Unselect room on panel change
                    this.uiPanelService.selectedObjectPanels.SubscribeQuietlyOnce(this, panels =>
                    {
                        this.UnSelectRoom(panels);
                    });
                    this.uiPanelService.selectedBuildingPanels.SubscribeQuietlyOnce(this, panels =>
                    {
                        this.UnSelectRoom(panels);
                    });
                }
            }
        }

        private void UnSelectRoom(IList<BasePanelModel> panels)
        {
            if (panels == null || panels.Find(panel => { return panel.ID == this.selectedRoomID; }) == null)
            {
                this.roomService.UnselectRoom(this.selectedRoomID);
            }
        }
    }
}