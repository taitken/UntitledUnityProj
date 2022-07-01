using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UtilityClasses;
using GameControllers.Services;
using GameControllers.Models;
using Zenject;

public class ActionController : MonoBehaviour2
{
    private IUnitActionService actionService;
    private IList<UnitActionModel> unitActionQueue;

    [Inject]
    public void Construct(IUnitActionService _actionService)
    {
        this.actionService = _actionService;
    }

    // Start is called before the first frame update
    void Start()
    {
        this.subscriptions.Add(this.actionService.actionQueue.Subscribe(queue =>
        {
            this.unitActionQueue = queue;
        })
        );
        this.actionService.AddAction(new UnitActionModel{ID = 1, actionCategory = eActionCategories.Build, actionName = "Test", priority = 1});
        this.actionService.AddAction(new UnitActionModel{ID = 2, actionCategory = eActionCategories.Dig, actionName = "Test2", priority = 2});
    }

    // Update is called once per frame
    void Update()
    {

    }

    void BeforeDestroy()
    {

    }
}
