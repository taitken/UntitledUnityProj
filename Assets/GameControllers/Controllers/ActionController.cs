using System.Collections.Generic;
using UnityEngine;
using GameControllers.Services;
using GameControllers.Models;
using UnitAction;
using Zenject;
using Unit.Models;
using UnityEngine.Tilemaps;

public class ActionController : MonoBehaviour2
{
    private ActionFactory actionFactory;
    private IBuildingService buildingService;
    private IItemObjectService itemService;
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
        this.buildingService = _buildingService;
        this.itemService = _itemService;
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

    private void CheckAndAssignOrder()
    {
        UnitModel unitWithoutOrder = this.currentUnits.Find(unit => { return unit.currentOrder == null; });
        if (unitWithoutOrder != null && this.unassignedOrders.Count > 0)
        {
            for (int i = 0; i < this.unassignedOrders.Count; i++)
            {
                if (this.PreOrderAssignCheck(this.unassignedOrders[i]))
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
    }

    // Action controller probably shouldnt know about order sub-types
    private bool PreOrderAssignCheck(UnitOrderModel order)
    {
        bool returnVal = true;
        switch (order.orderType)
        {
            case eOrderTypes.Build:
                // code block
                break;
            case eOrderTypes.Dig:
                // code block
                break;
            case eOrderTypes.Supply:
                SupplyOrderModel supplyOrder = order as SupplyOrderModel;
                returnVal = this.itemService.IsItemAvailable(supplyOrder.itemType);
                break;
            case eOrderTypes.Store:
                returnVal = this.buildingService.IsStorageAvailable();
                break;
        }
        return returnVal;
    }

    private void CreateAndBeginSequence(UnitModel unitModel)
    {
        ActionSequence actionSequence = this.actionFactory.CreateSequence(unitModel);
        if (actionSequence != null)
        {
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
