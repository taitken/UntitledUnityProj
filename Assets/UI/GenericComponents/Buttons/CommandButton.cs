using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using Building.Models;
using GameControllers.Services;
using GameControllers.Models;


namespace UI
{
    public class CommandButton : HiveBaseButton
    {
        public eMouseAction mouseType;
        public eBuildingType buildingType;
        private IUnitOrderService orderService;
        public Button buttonComponent;

        [Inject]
        public void Construct(IUnitOrderService _orderService)
        {
            this.orderService = _orderService;
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
            this.orderService.mouseAction.Set(new MouseActionModel(this.mouseType, this.buildingType));
        }
    }
}