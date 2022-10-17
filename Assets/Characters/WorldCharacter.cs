
using System.IO;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Globalization;
using System;
using System.Collections.Generic;
using UnityEngine;
using GameControllers.Services;
using GameControllers.Models;
using Characters.Utils;
using Zenject;
using Item;
using Unit.Models;
using Item.Models;
using UtilityClasses;
using UI.Services;
using UI.Models;

namespace Characters
{
    public class WorldCharacter : MonoBaseObject
    {
        public IEnvironmentService environmentService;
        public CharacterPathLine.Factory pathLineFactory;
        public CharacterPathLine pathingLine;
        protected IUnitOrderService orderService;
        protected IPathFinderService pathFinderService;
        protected IItemObjectService itemService;
        protected IUiPanelService contextWindowService;
        protected ItemObject carriedObj;
        protected BaseUnitState unitState;
        public ContactFilter2D movementFilter = new ContactFilter2D();
        protected List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();
        public UnitModel unitModel { get; set; }
        public virtual float collisionOffset { get; set; } = 0.00f;
        [Inject]
        public void Construct(IUnitOrderService _orderService,
                              IPathFinderService _pathFinderService,
                              IEnvironmentService _environmentService,
                              IItemObjectService _itemService,
                              IUiPanelService _contextWindowService,
                              CharacterPathLine.Factory _pathLineFactory,
                              UnitModel _unitModel
        )
        {
            this.movementFilter.SetLayerMask(LayerMask.GetMask("MineableLayer"));
            this.orderService = _orderService;
            this.pathFinderService = _pathFinderService;
            this.environmentService = _environmentService;
            this.contextWindowService = _contextWindowService;
            this.itemService = _itemService;
            this.pathLineFactory = _pathLineFactory;
            this.unitModel = _unitModel;
            this.unitState = UnitStateFactory.CreateUnitState(this.unitModel.unitState);

            // Listeners
            this.orderService.orders.Subscribe(this, this.HandleOrderUpdates);
            this.itemService.onItemPickupOrDropTrigger.Subscribe(this, this.OnItemPickupOrDrop);
            this.itemService.onItemStoreOrSupplyTrigger.SubscribeQuietly(this, this.OnItemStoreOrSupply);
            this.pathFinderService.OnPathFinderMapUpdate(this, this.CheckIfPathObstructed);
            this.unitModel.ListenForUpdates(this.ListenForModelUpdates);
        }

        public override BaseObjectModel GetBaseObjectModel()
        {
            return this.unitModel;
        }

        protected void Update()
        {
            this.CheckAndUpdateState();
            this.unitState.Update(this);
        }

        protected void FixedUpdate()
        {
            this.CheckAndUpdateState();
            this.UpdateNeeds();
            this.unitState.FixedUpdate(this);
        }

        private void CheckAndUpdateState()
        {
            if (this.unitModel.unitState != this.unitState.stateEnum)
            {
                this.unitState = UnitStateFactory.CreateUnitState(this.unitModel.unitState);
            }
        }

        protected void ListenForModelUpdates()
        {

        }

        private void UpdateNeeds()
        {
            this.unitModel.needsComponent.UpdateFullness(1);
        }
        private void HandleOrderUpdates(IList<UnitOrderModel> orders)
        {
            if (this.unitModel.currentOrder == null && this.unitModel.currentPath != null)
            {
                this.CancelMoving();
                this.DetachItemFromUnit();
            }
        }
        private void CheckIfPathObstructed(PathFinderMap newMap)
        {
            if (this.unitModel.currentPath != null)
            {
                bool obstructed = false;
                this.unitModel.currentPath.ForEach(pathStep =>
                {
                    Vector3Int cellPos = this.environmentService.LocalToCell(pathStep);
                    if (newMap.mapitems[cellPos.x, cellPos.y].impassable)
                    {
                        obstructed = true;
                    }
                });
                if (obstructed) this.CancelMoving();
            }
        }

        protected void CancelMoving()
        {
            this.unitModel.currentPath = null;
            if (this.pathingLine != null) this.pathingLine.Destroy();
        }

        protected bool CollisionCheck(Vector2 movement)
        {
            Physics2D.Raycast(this.transform.position, movement, movementFilter, castCollisions, this.unitModel.moveSpeed * (GameTime.fixedDeltaTime * 1f) + collisionOffset
            );
            return castCollisions.Filter(collision => { return collision.collider.gameObject.tag != "AllowMovement"; }).Count != 0;
        }

        private void OnItemStoreOrSupply(ItemObjectModel item)
        {
            if (this.carriedObj?.itemObjectModel.ID == item.ID) this.DetachItemFromUnit();
        }

        private void OnItemPickupOrDrop(ItemObjectModel item)
        {
            // Only picks up if not carrying item
            // To do -- Implement item switch logic eg. drop carried obj and pickup new obj
            if (this.unitModel.carriedItem != null && this.carriedObj == null)
            {
                this.AttachItemToUnit(this.itemService.GetItemObject(this.unitModel.carriedItem.ID));
            }
            if (this.unitModel.carriedItem == null && this.carriedObj != null)
            {
                this.DetachItemFromUnit();
            }
        }

        private void AttachItemToUnit(ItemObject itemObj)
        {
            if (itemObj == null)
            {
                unitModel.carriedItem = null;
            }
            else
            {
                this.carriedObj = itemObj;
                itemObj.transform.SetParent(this.transform);
                itemObj.transform.localPosition = new Vector3(0, 0, 0);
            }
        }

        private void DetachItemFromUnit()
        {
            this.carriedObj = null;
            this.unitModel.carriedItem = null;
            ItemObject foundItem = this.GetComponentInChildren<ItemObject>();
            if (foundItem) foundItem.transform.SetParent(null);
        }

        public override void OnSelect()
        {
            IList<BasePanelModel> panels = new List<BasePanelModel>();
            panels.Add(new ObjectPanelModel(this.unitModel.ID, "Drone", this.unitModel));
            this.uiPanelService.selectedObjectPanels.Set(panels);
        }

        public override void OnMouseEnter()
        {
            List<string> newContext = new List<string>();
            newContext.Add("Position: " + this.unitModel.position.ToString());
            newContext.Add("LocalPosition: " + this.unitModel.localPosition.ToString());
            this.contextWindowService.AddContext(new ObjectContextWindowModel(this.unitModel.ID, "Drone", newContext));
        }

        public override void OnMouseExit()
        {
            this.contextWindowService.RemoveContext(this.unitModel.ID);
        }

        public class Factory : PlaceholderFactory<UnitModel, WorldCharacter>
        {
        }
    }
}
