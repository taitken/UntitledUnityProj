using UnityEngine;

namespace System
{
    public abstract class BasePhysicalObjectModel : BaseModel
    {
        public BasePhysicalObjectModel(Vector3Int _position, decimal _mass) : base()
        {
            this.position = _position;
            this.mass = _mass;
        }
        public Vector3Int position { get; set; }
        public decimal mass { get; set; }
    }
}