using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;
using GameControllers.Services;
using GameControllers.Models;
using System.Collections.Generic;

public class ClickManager : MonoBehaviour2
{
    eMouseAction currentMouseAction;
    IUnitActionService actionService;
    [Inject]
    public void Construct(IUnitActionService _actionService)
    {
        this.actionService = _actionService;
        this.actionService.mouseAction.Subscribe(action =>
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
            this.actionService.mouseAction.Set(eMouseAction.None);
        }
    }

    List<RaycastHit2D> LeftClick(ContactFilter2D contactFilter)
    {
        Vector2 mousePosition = Mouse.current.position.ReadValue();
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(mousePosition);
        Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);
        List<RaycastHit2D> hitResults = new List<RaycastHit2D>();
        Physics2D.Raycast(mousePos2D, Vector2.zero, contactFilter, hitResults);
        return hitResults;
    }

    void ClickObject(RaycastHit2D hitObject)
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
    void ClickObject(List<RaycastHit2D> hitObjects)
    {
        if (hitObjects.Count > 0)
        {
            this.ClickObject(hitObjects[0]);
        }
    }

    void DigCommandClick()
    {
        ContactFilter2D filter = new ContactFilter2D();
        filter.SetLayerMask(LayerMask.GetMask("MineableLayer"));
        this.ClickObject(this.LeftClick(filter));
    }

    void BuildCommandClick()
    {
        ContactFilter2D filter = new ContactFilter2D();
        this.ClickObject(this.LeftClick(filter));
    }

    void CancelCommandClick()
    {
        ContactFilter2D filter = new ContactFilter2D();
        filter.SetLayerMask(LayerMask.GetMask("UnitOrderLayer"));
        this.ClickObject(this.LeftClick(filter));
    }
}
