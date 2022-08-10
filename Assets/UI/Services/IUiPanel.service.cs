using System;
using System.Collections.Generic;
using UnityEngine;
using UtilityClasses;
using GameControllers.Models;
using UI.Models;

namespace UI.Services
{
    public interface IUiPanelService
    {
        public ContextAssetFactory contextAssetFactory { get; set; }
        public MonoObseravable<IList<ContextWindowModel>> contextObseravable { get; set; }
        public MonoObseravable<IList<BasePanelModel>> selectedObjectPanels { get; set; }
        void SetContextAssetFactory(ContextAssetFactory assetFactory);
        void SetPanelAssetFactory(ObjectPanelAssetFactory assetFactory);
        ObjectPanelAssetFactory GetPanelAssetFactory();
        void AddContext(ContextWindowModel context);
        void RemoveContext(long modelID);
    }
}

