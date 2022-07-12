using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;
using GameControllers.Services;
using GameControllers.Models;
using System.Collections.Generic;

public class MouseActionController : MonoBehaviour2
{
    eMouseAction currentMouseAction;
    IUnitOrderService orderService;
    IList<RaycastHit2D> oldMouseOverHits = new List<RaycastHit2D>();
    [Inject]
    public void Construct(IUnitOrderService _orderService)
    {
        this.orderService = _orderService;
        this.orderService.mouseAction.Subscribe(action =>
        {
            this.currentMouseAction = action;
        });

    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        this.MouseOverCheck();
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            switch (this.currentMouseAction)
            {
                case eMouseAction.Build:
                    this.BuildCommandClick();
                    break;
                case eMouseAction.Dig:
                    this.DigCommandClick();
                    break;
                case eMouseAction.Cancel:
                    this.CancelCommandClick();
                    break;
            }
        }
        if (Mouse.current.rightButton.wasPressedThisFrame)
        {
            this.orderService.mouseAction.Set(eMouseAction.None);
        }
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
            filter.SetLayerMask(LayerMask.GetMask("MineableLayer", "BuildingLayer"));
            IList<RaycastHit2D> newHits = this.RayCastOnMouse(filter);
            newHits.ForEach(hit =>
            {
                if (hit.collider != null)
                {
                    if (hit.collider.gameObject.GetComponent<MonoBehaviour2>())
                    {
                        if (this.oldMouseOverHits.Find(oldHit => { return oldHit.collider.gameObject == hit.collider.gameObject; }) == default(RaycastHit2D))
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

    private List<RaycastHit2D> RayCastOnMouse(ContactFilter2D contactFilter)
    {
        Vector2 mousePosition = Mouse.current.position.ReadValue();
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(mousePosition);
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
    private void ClickObject(List<RaycastHit2D> hitObjects)
    {
        if (hitObjects.Count > 0)
        {
            this.ClickObject(hitObjects[0]);
        }
    }

    private void DigCommandClick()
    {
        ContactFilter2D filter = new ContactFilter2D();
        filter.SetLayerMask(LayerMask.GetMask("MineableLayer"));
        this.ClickObject(this.RayCastOnMouse(filter));
    }

    private void BuildCommandClick()
    {
        ContactFilter2D filter = new ContactFilter2D();
        this.ClickObject(this.RayCastOnMouse(filter));
    }

    private void CancelCommandClick()
    {
        ContactFilter2D filter = new ContactFilter2D();
        filter.SetLayerMask(LayerMask.GetMask("UnitOrderLayer"));
        this.ClickObject(this.RayCastOnMouse(filter));
    }
}
