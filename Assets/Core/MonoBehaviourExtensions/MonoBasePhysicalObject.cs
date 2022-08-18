using System;
using System.Collections.Generic;
using GameControllers.Models;
using GameControllers.Services;
using UnityEngine;
using UtilityClasses;
using Zenject;

namespace UnityEngine
{
    public class MonoBaseObject : MonoBehaviour2
    {
        public virtual BaseObjectModel GetBaseObjectModel()
        {
            Debug.LogException(new System.Exception("Get base object model not implemented for this object. Please implement GetBaseObjectModel."));
            return null;
        }
        public virtual void OnSelect()
        {

        }
    }
}