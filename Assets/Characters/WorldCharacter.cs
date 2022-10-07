
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
        protected IUnitOrderService orderService;
        protected IPathFinderService pathFinderService;
        protected IEnvironmentService environmentService;
        protected IItemObjectService itemService;
        protected IUiPanelService contextWindowService;
        protected CharacterPathLine.Factory pathLineFactory;
        protected CharacterPathLine pathingLine;
        protected Rigidbody2D rb;
        protected SpriteRenderer sr;
        protected Animator animator;
        protected ItemObject carriedObj;
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

            this.orderService.orders.Subscribe(this, this.HandleOrderUpdates);
            this.itemService.onItemPickupOrDropTrigger.Subscribe(this, this.OnItemPickupOrDrop);
            this.itemService.onItemStoreOrSupplyTrigger.SubscribeQuietly(this, this.OnItemStoreOrSupply);
            this.pathFinderService.OnPathFinderMapUpdate(this, this.CheckIfPathObstructed);

            rb = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
            sr = GetComponent<SpriteRenderer>();
        }

        public override BaseObjectModel GetBaseObjectModel()
        {
            return this.unitModel;
        }

        void Update()
        {
        }

        protected void FixedUpdate()
        {
            this.UpdateNeeds();
            this.CheckPathingAndMove();
        }

        private void UpdateNeeds()
        {
            this.unitModel.needsComponent.UpdateFullness(1);
        }

        private void CheckPathingAndMove()
        {
            if (this.unitModel.currentPath != null && this.unitModel.currentPath.Count > 0)
            {
                this.MoveUnit(new Vector2(1, 1));
                this.UpdateMoveLine();
            }
        }

        private void UpdateMoveLine()
        {
            IList<Vector3> newLinePath = this.unitModel.currentPath.Map(item => { return this.environmentService.CellToLocal(item); });
            newLinePath.Insert(0, this.gameObject.transform.position);
            if (this.pathingLine == null)
            {
                this.pathingLine = this.pathLineFactory.Create(newLinePath);
            }
            else
            {
                this.pathingLine.UpdateLine(newLinePath);
            }
        }

        private void UpdatePositions()
        {
            this.unitModel.localPosition = new Vector3(this.gameObject.transform.position.x + this.unitModel.spriteOffset, this.gameObject.transform.position.y + this.unitModel.spriteOffset);
            this.unitModel.position = this.environmentService.LocalToCell(this.unitModel.localPosition);
            if (this.unitModel.carriedItem != null && this.unitModel.carriedItem.itemState == ItemObjectModel.eItemState.OnCharacter) this.unitModel.carriedItem.position = this.unitModel.position;
        }

        private void HandleOrderUpdates(IList<UnitOrderModel> orders)
        {
            if (this.unitModel.currentOrder == null && this.unitModel.currentPath != null)
            {
                this.CancelMoving();
                this.DetachItemFromUnit();
            }
        }

        protected void MoveUnit(Vector2 distance)
        {
            if (distance != Vector2.zero)
            {
                Vector3 nextPoint = this.environmentService.CellToLocal(this.unitModel.currentPath[0]);
                Vector2 direction = this.gameObject.transform.position.GetDirection(nextPoint);
                Vector2 newPosition = rb.position + (distance * direction * this.unitModel.moveSpeed * GameTime.fixedDeltaTime);
                Vector2 overshootDistance = this.GetMovementOvershoot(direction, newPosition, nextPoint);
                this.UpdateSpriteDirection(direction);
                // Overshot movement location
                if (overshootDistance.x > 0 && direction.y == 0
                    || overshootDistance.y > 0 && direction.x == 0
                    || overshootDistance.y > 0 && overshootDistance.x > 0)
                {
                    this.rb.MovePosition(nextPoint);
                    this.UpdatePositions();
                    this.unitModel.currentPath.RemoveAt(0);
                    if (this.unitModel.currentPath.Count > 0
                        && (overshootDistance.x + overshootDistance.y) > 0.02f)
                    {
                        this.MoveUnit(overshootDistance / this.unitModel.moveSpeed / GameTime.fixedDeltaTime);
                    }
                }
                // Did not overshoot
                else
                {
                    this.rb.MovePosition(newPosition);
                    if ((Vector3)newPosition == nextPoint) this.unitModel.currentPath.RemoveAt(0);
                    this.UpdatePositions();
                }
            }
        }

        protected Vector2 GetMovementOvershoot(Vector2 direction, Vector2 newEndPosition, Vector3 currentPathPoint)
        {
            Vector2 overShootMovement = new Vector2(0, 0);
            if (direction.x > 0 && newEndPosition.x > currentPathPoint.x)
            {
                overShootMovement += new Vector2(newEndPosition.x - currentPathPoint.x, 0);
            }
            if (direction.x < 0 && newEndPosition.x < currentPathPoint.x)
            {
                overShootMovement += new Vector2(newEndPosition.x - currentPathPoint.x, 0);
            }
            if (direction.y > 0 && newEndPosition.y > currentPathPoint.y)
            {
                overShootMovement += new Vector2(0, newEndPosition.y - currentPathPoint.y);
            }
            if (direction.y < 0 && newEndPosition.y < currentPathPoint.y)
            {
                overShootMovement += new Vector2(0, newEndPosition.y - currentPathPoint.y);
            }
            return new Vector2(Math.Abs(overShootMovement.x), Math.Abs(overShootMovement.y));
        }

        protected void UpdateSpriteDirection(Vector2 movement)
        {
            if (movement.x > 0)
            {
                this.SetSpriteDirection(false, false, new Vector3(0, 0, -55));
            }
            else if (movement.x < 0)
            {
                this.SetSpriteDirection(true, false, new Vector3(0, 0, 65));
            }
            else if (movement.y > 0)
            {
                this.SetSpriteDirection(false, false, new Vector3(0, 0, 0));
            }
            else if (movement.y < 0)
            {
                this.SetSpriteDirection(false, true, new Vector3(0, 0, 0));
            }
        }

        private void SetSpriteDirection(bool flipX, bool flipY, Vector3 angle)
        {
            this.transform.eulerAngles = angle;
            this.GetComponent<SpriteRenderer>().flipX = flipX;
            this.GetComponent<SpriteRenderer>().flipY = flipY;
            this.GetComponentsInChildren<SpriteRenderer>().ForEach(sprite =>
            {
                sprite.flipX = flipX;
                sprite.flipY = flipY;
            });
        }
        private void CheckIfPathObstructed(PathFinderMap newMap)
        {
            if (this.unitModel.currentPath != null)
            {
                bool obstructed = false;
                this.unitModel.currentPath.ForEach(pathStep =>
                {
                    if (newMap.mapitems[pathStep.x, pathStep.y].impassable)
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
