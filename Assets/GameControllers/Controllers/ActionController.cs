using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UtilityClasses;
using GameControllers.Services;
using GameControllers.Models;
using Zenject;

public class ActionController : MonoBehaviour2
{
    private IUnitOrderService orderService;
    private IUnitService unitService;
    private IEnvironmentService environmentService;
    private IPathFinderService pathFinderService;
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
                          IPathFinderService _pathFinderService)
    {
        this.orderService = _orderService;
        this.unitService = _unitService;
        this.environmentService = _environmentService;
        this.pathFinderService = _pathFinderService;
        this.subscriptions.Add(this.orderService.orders.Subscribe(updatedOrders =>
        {
            IList<UnitOrderModel> removedOrders = updatedOrders.GetRemovedModels(this.currentOrders);
            this.unassignOrders(removedOrders);
            this.currentOrders = updatedOrders;
        }));
        this.subscriptions.Add(this.unitService.unitSubscribable.Subscribe(updatedUnits =>
        {
            this.currentUnits = updatedUnits;
        }));
        InvokeRepeating("CheckAndAssignOrder", 2.0f, 2.0f);
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void BeforeDestroy()
    {

    }

    void CheckAndAssignOrder()
    {
        UnitModel unitWithoutOrder = this.currentUnits.Find(unit => { return unit.currentOrder == null; });
        if (unitWithoutOrder != null && this.unassignedOrders.Count > 0)
        {
            unitWithoutOrder.currentOrder = this.unassignedOrders[0];
            Debug.Log("Order assigned");
            if (this.TryMoveUnit(unitWithoutOrder))
            {
                // Success
            }
            else
            {
                unitWithoutOrder.currentOrder = null;
            }
        }
        else
        {
            //Debug.Log("Order already assigned");
        }
    }

    void unassignOrders(IList<UnitOrderModel> ordersToUnassign)
    {
        this.currentUnits.ForEach(unit =>
        {
            if (unit.currentOrder != null && ordersToUnassign.Find(order => { return order.ID == unit.currentOrder.ID; }) != null)
            {
                unit.currentOrder = null;
            }
        });
    }

    public bool TryMoveUnit(UnitModel unit)
    {
        unit.currentPath = this.pathFinderService.FindPath(this.environmentService.LocalToCell(unit.position), unit.currentOrder.coordinates, this.pathFinderService.pathFinderMap.Get(), true);
        if (unit.currentPath != null)
        {
            unit.currentPath.RemoveAt(0);
        }
        return unit.currentPath != null;
    }
}
