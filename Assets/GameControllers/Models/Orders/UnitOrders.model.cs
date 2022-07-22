using System;
using System.Collections.Generic;
using Unit.Models;
using UnityEngine;

namespace GameControllers.Models
{
    public class UnitOrderModel : BaseModel
    {
        public UnitOrderModel(Vector3Int _coordinates, eOrderTypes _orderType, bool _displayIcon = true) : base()
        {
            this.coordinates = _coordinates;
            this.orderType = _orderType;
            this.displayIcon = _displayIcon;
        }
        public eOrderTypes orderType { get; set; }
        public Vector3Int coordinates { get; set; }
        public float prioritySetting { get; set; }
        public UnitModel assignedUnit { get; set; }
        public bool displayIcon { get; set; }

        public virtual bool IsUniqueCheck(IList<UnitOrderModel> orderList)
        {
            return orderList.Find(existingOrder => { return this.ID == existingOrder.ID || this.coordinates == existingOrder.coordinates; }) == null;
        }
    }
}

