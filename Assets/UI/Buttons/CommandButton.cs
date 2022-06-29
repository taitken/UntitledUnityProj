using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using GameControllers.Services;
using GameControllers.Models;


namespace UI
{
    public class CommandButton : HiveBaseButton
    {
        public eMouseAction mouseType;
        private IUnitActionService actionService;
        public Button buttonComponent;

        [Inject]
        public void Construct(IUnitActionService _actionService)
        {
            this.actionService = _actionService;
        }
        // Start is called before the first frame update
        void Start()
        {
            this.buttonComponent = GetComponent<Button>();
            this.buttonComponent.onClick.AddListener(ActivateMouseMode);
        }

        // Update is called once per frame
        void Update()
        {

        }

        void ActivateMouseMode()
        {
            this.actionService.mouseAction.Set(this.mouseType);
        }
    }
}