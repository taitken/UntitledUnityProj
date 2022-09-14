using System;
using System.Collections.Generic;
using GameControllers.Models;
using GameControllers.Services;
using Item.Models;
using ObjectComponents;
using UnityEngine;
using UtilityClasses;
using Zenject;

namespace UnityEngine
{
    public class MonoBaseObject : MonoBehaviour2
    {
        protected IItemObjectService itemObjectService;
        [Inject]
        public void Construct(IItemObjectService _itemService)
        {
            this.itemObjectService = _itemService;
        }
        public virtual BaseObjectModel GetBaseObjectModel()
        {
            Debug.LogException(new System.Exception("Get base object model not implemented for this object. Please implement GetBaseObjectModel."));
            return null;
        }
        public virtual void OnSelect()
        {

        }
        protected override void BeforeDeath()
        {
            ObjectCompositionComponent oc = this.GetBaseObjectModel().GetObjectComponent<ObjectCompositionComponent>();
            ObjectStorageComponent os = this.GetBaseObjectModel().GetObjectComponent<ObjectStorageComponent>();
            if (oc != null)
            {
                oc.GetComposition().ForEach(item =>
                {
                    this.itemObjectService.AddItem(new ItemObjectModel(this.GetBaseObjectModel().position, item, ItemObjectModel.eItemState.OnGround));
                });
            }
            if (os != null)
            {
                os.GetItems().ForEach(item =>
                {
                    this.itemObjectService.onItemPickupOrDropTrigger.Set(item);
                });
            }
        }
    }
}