using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameControllers.Services;
using GameControllers.Models;
using Zenject;

namespace UI
{
    public class MouseController : MonoBehaviour2
    {
        public Texture2D[] cursorTexures;
        private CursorMode cursorMode = CursorMode.Auto;
        private Vector2 hotSpot = Vector2.zero;
        private IUnitActionService actionService;
        [Inject]
        public void Construct(IUnitActionService _actionService)
        {
            this.actionService = _actionService;
            this.subscriptions.Add(this.actionService.mouseAction.Subscribe(action =>
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

