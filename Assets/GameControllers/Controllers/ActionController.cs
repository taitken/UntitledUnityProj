using System.Collections.Generic;
using UnityEngine;
using GameControllers.Services;
using GameControllers.Models;
using UnitAction;
using Zenject;
using Unit.Models;
using UnityEngine.Tilemaps;
using System;
using Characters;

public class ActionController : MonoBehaviour2
{
    private ActionFactory actionFactory;
    private IUnitOrderService unitOrderService;
    private IPathFinderService pathFinderService;
    private IList<IBaseService> services;
    private IList<ActionSequence> actionSequences = new List<ActionSequence>();
    private IList<UnitOrderModel> currentOrders = new List<UnitOrderModel>();
    private IList<UnitOrderModel> unassignedOrders
    {
        get
        {
            return this.currentOrders.Filter(order =>
            {
                return this.currentUnits.Find(unit =>
                {
                    return unit.currentOrder != null
                           && unit.currentOrder.ID == order.ID;
                }) == null && order.replaceableOrder == false;
            });
        }
    }
    private IList<UnitModel> currentUnits = new List<UnitModel>();

    [Inject]
    public void Construct(IUnitOrderService _orderService,
                          IUnitService _unitService,
                          IEnvironmentService _environmentService,
                          IPathFinderService _pathFinderService,
                          IBuildingService _buildingService,
                          IItemObjectService _itemService,
                          ICropService _cropService)
    {
        this.services = new List<IBaseService>() { _orderService, _unitService, _environmentService, _pathFinderService, _buildingService, _itemService, _cropService };
        this.pathFinderService = _pathFinderService;
        this.unitOrderService = _orderService;
        this.actionFactory = new ActionFactory(_pathFinderService, _environmentService, _orderService, _buildingService, _itemService, _cropService);
        _orderService.orders.Subscribe(this, this.HandleOrderUpdates);
        _unitService.unitObseravable.Subscribe(this, updatedUnits => { this.currentUnits = updatedUnits; });
        InvokeRepeating("CheckAndAssignOrder", 1f, 0.2f);
        InvokeRepeating("AssignIdleOrders", 1f, 0.1f);
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        this.actionSequences.ForEach(sequence => { sequence.Update(); });
    }

    void BeforeDestroy()
    {

    }

    private void HandleOrderUpdates(IList<UnitOrderModel> updatedOrders)
    {
        IList<UnitOrderModel> removedOrders = updatedOrders.GetRemovedModels(this.currentOrders);
        this.UnassignOrders(removedOrders);
        this.currentOrders = updatedOrders;
    }

    private void CheckAndAssignOrder()
    {
        this.currentUnits.Filter(unit => { return unit.currentOrder == null || unit.currentOrder.replaceableOrder; }).ForEach(unitWithoutOrder =>
        {
            if (unitWithoutOrder != null && this.unassignedOrders.Count > 0)
            {
                for (int i = 0; i < this.unassignedOrders.Count; i++)
                {
                    if (this.unassignedOrders[i].CanAssignToUnit(this.services, unitWithoutOrder))
                    {
                        if (unitWithoutOrder.currentOrder != null) this.unitOrderService.RemoveOrder(unitWithoutOrder.currentOrder.ID);
                        unitWithoutOrder.currentOrder = this.unassignedOrders[i];
                        this.CreateAndBeginSequence(unitWithoutOrder);
                        break;
                    }
                }
            }
            else
            {
                //Debug.Log("Order already assigned");
            }
        });
    }

    private void AssignIdleOrders()
    {
        this.currentUnits.Filter(unit => { return unit.currentOrder == null && unit.unitState == eUnitState.Idle; }).ForEach(idleUnit =>
        {
            int roll = UnityEngine.Random.Range(0, 100);
            if (roll < 10)
            {
                Vector3Int endPos = this.GetIdleWalkPath(idleUnit);
                if (endPos != default(Vector3Int))
                {
                    WanderOrderModel wanderOrder = new WanderOrderModel(this.GetIdleWalkPath(idleUnit), false);
                    if (wanderOrder.CanAssignToUnit(this.services, idleUnit))
                    {
                        this.unitOrderService.AddOrder(wanderOrder);
                        idleUnit.currentOrder = wanderOrder;
                        this.CreateAndBeginSequence(idleUnit);
                    }
                }
            }
        });

    }

    private Vector3Int GetIdleWalkPath(UnitModel unit)
    {
        int yRand = UnityEngine.Random.Range(-5, 5);
        int xRand = UnityEngine.Random.Range(-5, 5);
        Vector3Int newEndPos = new Vector3Int(unit.position.x + xRand, unit.position.y + yRand);
        if (this.pathFinderService.CanPathTo(unit.position, newEndPos, false) && !this.pathFinderService.CheckIfImpassable(newEndPos))
        {
            return newEndPos;
        }
        else
        {
            return default(Vector3Int);
        }
    }

    private void CreateAndBeginSequence(UnitModel unitModel)
    {
        ActionSequence actionSequence = this.actionFactory.CreateSequence(unitModel);
        if (actionSequence != null)
        {
            actionSequence.onCancel.OnEmit(() =>
            {
                this.UnassignOrders(new List<UnitOrderModel> { unitModel.currentOrder });
            });
            actionSequence.Begin();
            this.actionSequences.Add(actionSequence);
        }
    }

    void UnassignOrders(IList<UnitOrderModel> ordersToUnassign)
    {
        this.currentUnits.ForEach(unit =>
        {
            if (unit.currentOrder != null && ordersToUnassign.Find(order => { return order.ID == unit.currentOrder.ID; }) != null)
            {
                unit.currentOrder = null;
                unit.currentPath.Clear();
            }
        });
        // Remove Sequences (prevents updating)
        this.actionSequences = this.actionSequences.Filter(sequence =>
        {
            return ordersToUnassign.Find(order =>
            {
                return order.ID == sequence.unitOrder.ID;
            }) == null;
        });
    }
}
