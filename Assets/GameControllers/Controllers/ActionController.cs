using System.Collections.Generic;
using UnityEngine;
using GameControllers.Services;
using GameControllers.Models;
using UnitAction;
using Zenject;
using Unit.Models;

public class ActionController : MonoBehaviour2
{
    private ActionFactory actionFactory;
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
                          IItemObjectService _itemService)
    {
        this.actionFactory = new ActionFactory(_pathFinderService, _environmentService, _orderService, _buildingService, _itemService);
        this.subscriptions.Add(_orderService.orders.Subscribe(updatedOrders =>
        {
            IList<UnitOrderModel> removedOrders = updatedOrders.GetRemovedModels(this.currentOrders);
            this.UnassignOrders(removedOrders);
            this.currentOrders = updatedOrders;
        }));
        this.subscriptions.Add(_unitService.unitObseravable.Subscribe(updatedUnits =>
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
        this.actionSequences.ForEach(sequence =>
        {
            sequence.Update();
        });
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
            this.CreateAndBeginSequence(unitWithoutOrder);
        }
        else
        {
            //Debug.Log("Order already assigned");
        }
    }

    private void CreateAndBeginSequence(UnitModel unitModel)
    {
        ActionSequence actionSequence = this.actionFactory.CreateSequence(unitModel);
        actionSequence.Begin();
        this.actionSequences.Add(actionSequence);
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
