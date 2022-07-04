using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;
using GameControllers.Services;

public class ClickManager : MonoBehaviour2
{
    IUnitActionService actionService;
    [Inject]
    public void Construct(IUnitActionService _actionService)
    {
        this.actionService = _actionService;

    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Vector2 mousePosition = Mouse.current.position.ReadValue();
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
            if (hit.collider != null)
            {
                if (hit.collider.gameObject.GetComponent<MonoBehaviour2>())
                {
                    hit.collider.gameObject.GetComponent<MonoBehaviour2>().OnClickedByUser();
                };
            }
        }
        if (Mouse.current.rightButton.wasPressedThisFrame)
        {
            this.actionService.mouseAction.Set(GameControllers.Models.eMouseAction.None);
        }
    }

}
