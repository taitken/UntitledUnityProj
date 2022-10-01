using System.Collections.Generic;
using Item.Models;
using ObjectComponents;
using UnityEngine;
using UtilityClasses;

namespace System
{
    public abstract class BaseObjectModel : BaseModel
    {
        protected IList<ObjectComponent> objectComponents { get; set; }
        private EventEmitter updateNotifier = new EventEmitter();
        public Vector3Int position { get; set; }
        public string objectDescription { get; set; }
        public decimal mass { get { return this.GetObjectComponent<ObjectCompositionComponent>().GetMass(); } }
        public float spriteOffset { get; set; } = 0;
        public BaseObjectModel(Vector3Int _position, IList<ItemObjectMass> objectComp) : base()
        {
            this.position = _position;
            this.objectComponents = new List<ObjectComponent>();
            this.objectComponents.Add(new ObjectCompositionComponent(objectComp));
        }

        public void ListenForUpdates(Action updateAction)
        {
            this.updateNotifier.OnEmit(updateAction);
        }

        public void NotifyModelUpdate()
        {
            this.updateNotifier.Emit();
        }

        public T GetObjectComponent<T>() where T : ObjectComponent
        {
            for (int i = 0; i < this.objectComponents.Count; i++)
            {
                if (this.objectComponents[i] is T)
                {
                    return this.objectComponents[i] as T;
                }
            }
            return null;
        }
    }
}