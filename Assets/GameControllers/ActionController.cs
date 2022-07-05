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
    }

    // Update is called once per frame
    void Update()
    {

    }

    void BeforeDestroy()
    {

    }
}
