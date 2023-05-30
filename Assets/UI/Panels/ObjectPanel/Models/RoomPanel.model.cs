using System;
using System.Collections.Generic;
using Building.Models;
using Room.Models;
using UnityEngine;

namespace UI.Models
{
    public class RoomPanelModel : BasePanelModel
    {
        public RoomModel roomModel;
        public RoomPanelModel(long _objectID, string _title, RoomModel _roomModel) : base(_objectID, _title, ePanelTypes.RoomInfo)
        {
            this.roomModel = _roomModel;
        }
    }
}

