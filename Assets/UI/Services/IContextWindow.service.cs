using System;
using System.Collections.Generic;
using UnityEngine;
using UtilityClasses;
using GameControllers.Models;
using UI.Models;

namespace UI.Services
{
    public interface IContextWindowService
    {
        public Obseravable<IList<ContextWindowModel>> contextObseravable { get; set; }
        void AddContext(ContextWindowModel context);
        void RemoveContext(long modelID);
    }
}

