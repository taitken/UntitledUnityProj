using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UtilityClasses;
using GameControllers.Services;
using GameControllers.Models;
using System;

public interface IUnitAction 
{
    Action completeCondition {get; set;}
}
