using System;
using Unit.Models;
using UnityEngine;

namespace GameControllers.Models
{
    public class UnitOrderModel : BaseModel
    {
        public UnitOrderModel(Vector3Int _coordinates, eOrderTypes _orderType) : base()
        {
            this.coordinates = _coordinates;
            this.orderType = _orderType;
        }
        public eOrderTypes orderType { get; set; }
        public Vector3Int coordinates { get; set; }
        public float prioritySetting { get; set; }
        public UnitModel assignedUnit { get; set; }
    }
}

