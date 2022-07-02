
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameControllers.Services;
using GameControllers.Models;
using Zenject;

namespace Characters
{
    public class WorldCharacter : MonoBehaviour2
    {
        protected UnitOrderModel currentOrder;
        protected IUnitActionService actionService;
        [Inject]
        public void Construct(IUnitActionService _actionService)
        {
            this.actionService = _actionService;
            InvokeRepeating("CheckAndAssignOrder", 2.0f, 2.0f);
            this.actionService.orders.Subscribe(orders =>{
                this.currentOrder = orders.Find(order =>{return order.ID == this.currentOrder?.ID;});
            });
        }

        void Start()
        {
        }

        void Update()
        {

        }

        void CheckAndAssignOrder()
        {
            if (currentOrder == null)
            {
                this.currentOrder = this.actionService.GetNextOrder();
                if (currentOrder != null)
                {
                    //Debug.Log("Order assigned");
                } else {
                    //Debug.Log("No orders to assign");
                }
            } else {
                    //Debug.Log("Order already assigned");
            }
        }
    }
}
