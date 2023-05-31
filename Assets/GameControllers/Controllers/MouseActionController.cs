using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;
using GameControllers.Services;
using GameControllers.Models;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UI.Services;
using Environment;

namespace GameControllers
{
    public class MouseActionController : MonoBehaviour2
    {
        MouseActionModel currentMouseAction;
        IUnitOrderService orderService;
        IEnvironmentService environmentService;
        IUiPanelService uiPanelService;
        private MonoBaseObject previouslySelectedObject;
        private const float DRAG_TIME_LENGTH = 0.2f;
        private float leftClickDownDuration;
        private Vector3 dragClickStart;
        private DragEventModel dragEvent;
        IList<RaycastHit2D> oldMouseOverHits = new List<RaycastHit2D>();
        [Inject]
        public void Construct(IUnitOrderService _orderService,
                                IEnvironmentService _environmentService,
                                IUiPanelService _uiPanelService)
        {
            this.orderService = _orderService;
            this.environmentService = _environmentService;
            this.uiPanelService = _uiPanelService;
            this.orderService.mouseAction.Subscribe(this, action => { this.currentMouseAction = action; });
            this.leftClickDownDuration = 0;

        }
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            this.MouseOverCheck();
            if (!isMouseOverUI())
            {
                if (Mouse.current.leftButton.isPressed)
                {
                    if (this.dragClickStart == default(Vector3)) this.dragClickStart = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
                    this.leftClickDownDuration += Time.deltaTime;
                    if (this.leftClickDownDuration > 0 && this.leftClickDownDuration > DRAG_TIME_LENGTH) this.HandleLeftDrag();
                }
                else
                {
                    this.dragClickStart = default(Vector3);
                    if (this.leftClickDownDuration > 0 && this.leftClickDownDuration < DRAG_TIME_LENGTH) this.HandleLeftClick();
                    if (leftClickDownDuration > 0)
                    {
                        this.leftClickDownDuration = 0;
                        if (this.dragEvent != null)
                        {
                            this.HandleLeftDragEnd();
                        }
                    }
                }
            }
            if (Mouse.current.rightButton.wasPressedThisFrame)
            {
                this.HandleRightClick();
            }
        }

        private void HandleLeftClick()
        {
            switch (this.currentMouseAction.mouseType)
            {
                case eMouseAction.None:
                    this.SelectClick();
                    break;
                case eMouseAction.Build:
                    this.CommandClick(false, "BuildingLayer");
                    break;
                case eMouseAction.Dig:
                    this.CommandClick(true, "MineableLayer");
                    break;
                case eMouseAction.Store:
                    this.CommandClick(true, "ItemLayer");
                    break;
                case eMouseAction.Cancel:
                    this.CommandClick(true, "UnitOrderLayer", "BuildingLayer");
                    break;
                case eMouseAction.Deconstruct:
                    this.CommandClick(true, "BuildingLayer");
                    break;
            }
        }

        private void HandleRightClick()
        {
            this.orderService.mouseAction.Set(new MouseActionModel(eMouseAction.None));
            this.uiPanelService.ClearSelectedPanels();
        }

        private void HandleLeftDrag()
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            if (this.dragEvent == null)
            {
                this.dragEvent = new DragEventModel(this.dragClickStart, this.RayCastOnMouse(new ContactFilter2D()).Map(hit => { return hit.collider.gameObject; }));
            }
            this.dragEvent.currentDragLocation = mousePos;
            this.dragEvent.draggedObjects.ForEach(obj =>
            {
                if (obj != null) obj.GetComponent<MonoBehaviour2>().OnDrag(this.dragEvent);
            });
        }

        private void HandleLeftDragEnd()
        {
            this.dragEvent.draggedObjects.ForEach(obj => { if (obj != null) obj.GetComponent<MonoBehaviour2>().OnDragEnd(this.dragEvent); });
            switch (this.currentMouseAction.mouseType)
            {
                case eMouseAction.Build:
                    this.DragClick(this.dragEvent, "BuildingLayer");
                    break;
                case eMouseAction.Dig:
                    this.DragClick(this.dragEvent, "MineableLayer");
                    break;
                case eMouseAction.Store:
                    this.DragClick(this.dragEvent, "ItemLayer");
                    break;
                case eMouseAction.Cancel:
                    this.DragClick(this.dragEvent, "UnitOrderLayer", "BuildingLayer");
                    break;
                case eMouseAction.Deconstruct:
                    this.DragClick(this.dragEvent, "BuildingLayer");
                    break;
            }
            this.dragEvent = null;
        }

        private bool isMouseOverUI()
        {
            return UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject();
        }

        private void MouseOverCheck()
        {
            if (!isMouseOverUI())
            {
                ContactFilter2D filter = new ContactFilter2D();
                filter.SetLayerMask(LayerMask.GetMask("MineableLayer", "BuildingLayer", "ItemLayer", "CharacterLayer"));
                IList<RaycastHit2D> newHits = this.RayCastOnMouse(filter);
                newHits.ForEach(hit =>
                {
                    if (hit.collider != null)
                    {
                        if (hit.collider.gameObject.GetComponent<MonoBehaviour2>())
                        {
                            if (this.oldMouseOverHits.Find(oldHit => { return oldHit != default(RaycastHit2D) && oldHit.collider.gameObject == hit.collider.gameObject; }) == default(RaycastHit2D))
                            {
                                hit.collider.gameObject.GetComponent<MonoBehaviour2>().OnMouseEnter();
                            }
                            hit.collider.gameObject.GetComponent<MonoBehaviour2>().OnMouseOver();
                        };
                    }
                });
                this.oldMouseOverHits.ForEach(oldHit =>
                {
                    if (oldHit.collider != null)
                    {
                        if (oldHit.collider.gameObject.GetComponent<MonoBehaviour2>())
                        {
                            if (newHits.Find(newHit => { return oldHit.collider.gameObject == newHit.collider.gameObject; }) == default(RaycastHit2D))
                            {
                                oldHit.collider.gameObject.GetComponent<MonoBehaviour2>().OnMouseExit();
                            }
                        };
                    }
                });
                this.oldMouseOverHits = newHits;
            }
            else
            {
                this.oldMouseOverHits.ForEach(hit => { hit.collider.gameObject.GetComponent<MonoBehaviour2>().OnMouseExit(); });
                this.oldMouseOverHits = new List<RaycastHit2D>();
            }
        }

        private List<RaycastHit2D> BoxCastOnDragEvent(DragEventModel dragEvent, ContactFilter2D contactFilter)
        {
            List<RaycastHit2D> hitResults = new List<RaycastHit2D>();
            float xDragStart = dragEvent.initialDragLocation.x;
            float yDragStart = dragEvent.initialDragLocation.y;
            float xDragEnd = dragEvent.currentDragLocation.x;
            float yDragEnd = dragEvent.currentDragLocation.y;

            float xOrigin = xDragStart < xDragEnd ? xDragStart : xDragEnd;
            float xWidth = xDragStart < xDragEnd ? xDragEnd - xDragStart : xDragStart - xDragEnd;
            float yOrigin = yDragStart < yDragEnd ? yDragStart : yDragEnd;
            float yWidth = yDragStart < yDragEnd ? yDragEnd - yDragStart : yDragStart - yDragEnd;
            Vector2 origin = new Vector2(xOrigin, yOrigin);
            Vector2 size = new Vector2(xWidth, yWidth);

            Physics2D.BoxCast(new Vector2(origin.x + size.x / 2, origin.y + size.y / 2), size, 0, new Vector2(0, 0), contactFilter, hitResults, 0);
            return hitResults;
        }

        private List<RaycastHit2D> RayCastOnMouse(ContactFilter2D contactFilter)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);
            List<RaycastHit2D> hitResults = new List<RaycastHit2D>();
            Physics2D.Raycast(mousePos2D, Vector2.zero, contactFilter, hitResults);
            return hitResults;
        }


        private void ClickObject(RaycastHit2D hitObject)
        {
            if (hitObject.collider != null)
            {
                if (hitObject.collider.gameObject.GetComponent<MonoBehaviour2>())
                {
                    hitObject.collider.gameObject.GetComponent<MonoBehaviour2>().OnClickedByUser();
                };
            }
        }

        // Checks a Raycast list and clicks the first object
        private void ClickObject(IList<RaycastHit2D> hitObjects)
        {
            // Prioritise non layers
            hitObjects = hitObjects.Filter(obj => { return obj.collider.gameObject.GetComponent<LayerCollider>() == null; });
            if (hitObjects.Count > 0)
            {
                this.ClickObject(hitObjects[0]);
            }
        }

        // Checks a Raycast list and clicks the first object
        private void ClickObjects(List<RaycastHit2D> hitObjects)
        {
            hitObjects.ForEach(hit => { this.ClickObject(hit); });
        }

        private void DragClick(DragEventModel dragEvent, params string[] layers)
        {
            ContactFilter2D filter = new ContactFilter2D();
            filter.SetLayerMask(LayerMask.GetMask(layers));
            this.ClickObjects(this.BoxCastOnDragEvent(dragEvent, filter));
        }

        private void CommandClick(bool onlyClickFirstObject, params string[] layers)
        {
            ContactFilter2D filter = new ContactFilter2D();
            filter.SetLayerMask(LayerMask.GetMask(layers));
            if (onlyClickFirstObject)
            {
                this.ClickObject(this.RayCastOnMouse(filter));
            }
            else
            {
                this.ClickObjects(this.RayCastOnMouse(filter));
            }
        }

        private void SelectClick()
        {
            bool hitFound = false;
            int previousHitIndex = -1;
            ContactFilter2D filter = new ContactFilter2D();
            IList<MonoBaseObject> hitObjects = this.RayCastOnMouse(filter)
                                                .Filter(hit => { return hit.collider != null && hit.collider.gameObject.GetComponent<MonoBaseObject>(); })
                                                .Map(obj => { return obj.collider.gameObject.GetComponent<MonoBaseObject>(); });
            hitObjects.ForEach((obj, index) => { if (this.previouslySelectedObject == obj) previousHitIndex = index; });
            if (previousHitIndex == hitObjects.Count - 1) previousHitIndex = -1;
            hitObjects.ForEach((hitObject, index) =>
            {
                if (hitFound == false && index > previousHitIndex)
                {
                    this.previouslySelectedObject = hitObject;
                    hitObject.OnSelect();
                    hitFound = true;
                }
            });
            if (!hitFound)
            {
                this.uiPanelService.ClearSelectedPanels();
            }
        }
    }
}