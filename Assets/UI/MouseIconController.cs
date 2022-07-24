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
        private IBuildingService buildingService;
        [Inject]
        public void Construct(IUnitOrderService _orderService,
                                IBuildingService _buildingService)
        {
            this.orderService = _orderService;
            this.buildingService = _buildingService;
            this.subscriptions.Add(this.orderService.mouseAction.Subscribe(action =>
            {
                this.setMouseIcon(action.mouseType);
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
            switch (action)
            {
                case eMouseAction.None:
                    {
                        Cursor.SetCursor(null, this.hotSpot, this.cursorMode);
                        break;
                    }
                default:
                    {
                        Cursor.SetCursor(this.cursorTexures[(int)action], this.hotSpot, this.cursorMode);
                        break;
                    }
            }
        }
    }
}

