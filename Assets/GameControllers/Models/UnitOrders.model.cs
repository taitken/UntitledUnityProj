using System;
using UnityEngine;

namespace GameControllers.Models
{
    public class UnitOrderModel : BaseModel
    {
        public UnitOrderModel(Vector3 _coordinates, eMouseAction mouseType) : base()
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
                default:
                    // code block
                    break;
            }
        }
        public eOrderTypes orderType { get; set; }
        public Vector3 coordinates { get; set; }
        public float prioritySetting { get; set; }
    }
}

