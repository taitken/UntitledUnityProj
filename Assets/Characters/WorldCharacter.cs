
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
        protected IPathFinderService pathFinderService;
        protected IEnvironmentService environmentService;
        protected PathFinderMap pathFindMap;
        protected IList<Vector3Int> currentPath;
        [Inject]
        public void Construct(IUnitActionService _actionService,
                              IPathFinderService _pathFinderService,
                              IEnvironmentService _environmentService
        )
        {
            this.actionService = _actionService;
            this.pathFinderService = _pathFinderService;
            this.environmentService = _environmentService;
            InvokeRepeating("CheckAndAssignOrder", 2.0f, 2.0f);
            this.subscriptions.Add(this.actionService.orders.Subscribe(orders =>
            {
                this.currentOrder = orders.Find(order => { return order.ID == this.currentOrder?.ID; });
            }));
            this.pathFinderService.pathFinderMap.Subscribe(map =>
            {
                this.pathFindMap = map;
            });
        }

        void Start()
        {
        }

        void Update()
        {

        }

        public bool TryMoveTo(Vector3Int endPos)
        {
            this.currentPath = this.pathFinderService.FindPath(this.environmentService.LocalToCell(this.gameObject.transform.position), endPos, this.pathFindMap);
            Debug.Log(this.currentPath);
            return this.currentPath != null;
        }

        void CheckAndAssignOrder()
        {
            if (this.currentOrder == null)
            {
                this.currentOrder = this.actionService.GetNextOrder();
                if (this.currentOrder != null)
                {
                    Debug.Log("Order assigned");
                    this.TryMoveTo(this.currentOrder.coordinates);
                }
                else
                {
                    //Debug.Log("No orders to assign");
                }
            }
            else
            {
                //Debug.Log("Order already assigned");
            }
        }
    }
}
