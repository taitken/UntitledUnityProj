using System;
using Unit.Models;
using UnityEngine;

namespace GameControllers.Models
{
    public class UnitOrderModel : BaseModel
    {
        public UnitOrderModel(Vector3Int _coordinates, eMouseAction mouseType) : base()
        {
            this.coordinates = _coordinates;
            switch (mouseType)
            {
                case eMouseAction.Dig:
                    this.orderType = eOrderTypes.Dig;
                    break;
                case eMouseAction.Build:
                    this.orderType = eOrderTypes.Build;
                    break;
                case eMouseAction.Store:
                    this.orderType = eOrderTypes.Store;
                    break;
                default:
                    // code block
                    break;
            }
        }
        public eOrderTypes orderType { get; set; }
        public Vector3Int coordinates { get; set; }
        public float prioritySetting { get; set; }
        public UnitModel assignedUnit { get; set; }
    }
}

