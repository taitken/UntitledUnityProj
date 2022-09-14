using System.Collections.Generic;
using UnityEngine;
using GameControllers.Services;
using GameControllers.Models;
using UnitAction;
using Zenject;
using Unit.Models;
using UnityEngine.Tilemaps;
using System;

public class ActionController : MonoBehaviour2
{
    private ActionFactory actionFactory;
    private IBuildingService buildingService;
    private IItemObjectService itemService;
    private IList<IBaseService> services;
    private IList<ActionSequence> actionSequences = new List<ActionSequence>();
    private IList<UnitOrderModel> currentOrders = new List<UnitOrderModel>();
    private IList<UnitOrderModel> unassignedOrders
    {
        get
        {
            return this.currentOrders.Filter(order => { return this.currentUnits.Find(unit => { return unit.currentOrder != null && unit.currentOrder.ID == order.ID; }) == null; });
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
        this.actionFactory = new ActionFactory(_pathFinderService, _environmentService, _orderService, _buildingService, _itemService, _cropService);
        _orderService.orders.Subscribe(this, this.HandleOrderUpdates);
        _unitService.unitObseravable.Subscribe(this, updatedUnits => { this.currentUnits = updatedUnits; });
        InvokeRepeating("CheckAndAssignOrder", 1f, 1f);
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
        this.currentUnits.Filter(unit => { return unit.currentOrder == null; }).ForEach(unitWithoutOrder =>
        {
            if (unitWithoutOrder != null && this.unassignedOrders.Count > 0)
            {
                for (int i = 0; i < this.unassignedOrders.Count; i++)
                {
                    if (this.unassignedOrders[i].CanAssignToUnit(this.services, unitWithoutOrder))
                    {
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
            }
        });
        this.actionSequences = this.actionSequences.Filter(sequence => { return ordersToUnassign.Find(order => { return order.ID == sequence.unitOrder.ID; }) == null; });
    }
}
