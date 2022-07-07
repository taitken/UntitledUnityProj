using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameControllers.Services;
using GameControllers.Models;
using Zenject;

namespace UI
{
    public class MouseIconController : MonoBehaviour2
    {
        public Texture2D[] cursorTexures;
        private CursorMode cursorMode = CursorMode.Auto;
        private Vector2 hotSpot = Vector2.zero;
        private IUnitOrderService orderService;
        [Inject]
        public void Construct(IUnitOrderService _orderService)
        {
            this.orderService = _orderService;
            this.subscriptions.Add(this.orderService.mouseAction.Subscribe(action =>
            {
                this.setMouseIcon(action);
            })
            );
        }
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        void setMouseIcon(eMouseAction action)
        {
            if (action == eMouseAction.None)
            {
                Cursor.SetCursor(null, this.hotSpot, this.cursorMode);
            }
            else
            {
                Cursor.SetCursor(this.cursorTexures[(int)action], this.hotSpot, this.cursorMode);
            }
        }
    }
}

