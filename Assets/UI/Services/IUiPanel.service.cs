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
        public MonoObseravable<IList<BasePanelModel>> selectedBuildingPanels { get; set; }
        void SetContextAssetFactory(ContextAssetFactory assetFactory);
        void SetPanelAssetFactory(ObjectPanelAssetFactory assetFactory);
        BasePanel CreatePanelWindow(RectTransform parentTransform, BasePanelModel panelWindowModel, IList<IBaseService> services);
        void AddContext(ContextWindowModel context);
        void RemoveContext(long modelID);
        void RemovePanelsForObject(long modelID);
    }
}

