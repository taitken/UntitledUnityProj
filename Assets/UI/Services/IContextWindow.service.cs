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
        public ContextAssetFactory contextAssetFactory { get; set; }
        public MonoObseravable<IList<ContextWindowModel>> contextObseravable { get; set; }
        void SetAssetFactory(ContextAssetFactory assetFactory);
        void AddContext(ContextWindowModel context);
        void RemoveContext(long modelID);
    }
}

