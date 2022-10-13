using System;
using System.Collections.Generic;
using GameControllers.Services;
using Item.Models;
using ObjectComponents;
using UI.Services;
using Zenject;

namespace UnityEngine
{
    public abstract class MonoBaseObject : MonoBehaviour2
    {
        protected IItemObjectService itemObjectService;
        protected IUiPanelService uiPanelService;

        [Inject]
        public void Construct(IItemObjectService _itemService,
                                IUiPanelService _uiPanelService)
        {
            this.itemObjectService = _itemService;
            this.uiPanelService = _uiPanelService;
        }
        public abstract BaseObjectModel GetBaseObjectModel();

        // Not to be overridden. Use Awake() instead.
        public void Start()
        {
            BaseObjectModel test = this.GetBaseObjectModel();
            if (this.GetBaseObjectModel() != null)
            {
                ObjectHitPointsComponent hitPointsComponent = this.GetBaseObjectModel().GetObjectComponent<ObjectHitPointsComponent>();
                if (hitPointsComponent != null)
                {
                    hitPointsComponent.OnZeroHitPoints(this.Destroy);
                }
            }
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
                    if (item.itemType != eItemType.OrganicMass)
                    {
                        this.itemObjectService.AddItemToWorld(new ItemObjectModel(this.GetBaseObjectModel().position, item, ItemObjectModel.eItemState.OnGround));
                    }
                });
            }
            if (os != null)
            {
                os.GetItems().ForEach(item =>
                {
                    item.itemState = ItemObjectModel.eItemState.OnGround;
                    this.itemObjectService.onItemPickupOrDropTrigger.Set(item);
                });
            }
            if (this.GetBaseObjectModel() != null) this.uiPanelService.RemoveContext(this.GetBaseObjectModel().ID);
            if (this.GetBaseObjectModel() != null) this.uiPanelService.RemovePanelsForObject(this.GetBaseObjectModel().ID);
        }
    }
}