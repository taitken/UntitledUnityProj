using System.Collections.Generic;
using ObjectComponents;
using UnityEngine;

namespace System
{
    public abstract class BaseObjectModel : BaseModel
    {
        protected IList<ObjectComponent> objectComponents { get; set; }
        public Vector3Int position { get; set; }
        public decimal mass { get; set; }
        public BaseObjectModel(Vector3Int _position, decimal _mass) : base()
        {
            this.position = _position;
            this.mass = _mass;
            this.objectComponents = new List<ObjectComponent>();
        }

        public T GetObjectComponent<T>() where T: ObjectComponent
        {
            for (int i = 0; i < this.objectComponents.Count ; i++)
            {
                if(this.objectComponents[i] is T)
                {
                    return this.objectComponents[i] as T;
                }
            }
            return null;
        }

    }
}