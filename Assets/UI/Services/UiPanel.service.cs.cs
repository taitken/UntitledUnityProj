using System;
using System.Collections.Generic;
using UtilityClasses;
using UI.Models;
using UI.Services;
using UnityEngine;

namespace UI.Services
{
    public class UiPanelService : IUiPanelService
    {
        public ContextAssetFactory contextAssetFactory { get; set; }
        private ObjectPanelAssetFactory panelAssetFactory { get; set; }
        public UiPanelService()
        {

        }
        public MonoObseravable<IList<ContextWindowModel>> contextObseravable { get; set; } = new MonoObseravable<IList<ContextWindowModel>>(new List<ContextWindowModel>());
        public MonoObseravable<IList<BasePanelModel>> selectedObjectPanels { get; set; } = new MonoObseravable<IList<BasePanelModel>>(null);
        public MonoObseravable<IList<BasePanelModel>> selectedBuildingPanels { get; set; } = new MonoObseravable<IList<BasePanelModel>>(null);

        public void SetContextAssetFactory(ContextAssetFactory assetFactory)
        {
            if (this.contextAssetFactory == null)
            {
                this.contextAssetFactory = assetFactory;
            }
            else
            {
                Debug.LogException(new System.Exception("Error setting a new asset factory in the UI service. Only one context window service may be set"));
            }
        }
        public void SetPanelAssetFactory(ObjectPanelAssetFactory assetFactory)
        {
            if (this.panelAssetFactory == null)
            {
                this.panelAssetFactory = assetFactory;
            }
            else
            {
                Debug.LogException(new System.Exception("Error setting a new asset factory in the UI service. Only one context window service may be set"));
            }
        }
        public void ClearSelectedPanels()
        {
            this.selectedObjectPanels.Set(null);
            this.selectedBuildingPanels.Set(null);
        }
        public BasePanel CreatePanelWindow(RectTransform parentTransform, BasePanelModel panelWindowModel, IList<IBaseService> services)
        {
            return this.panelAssetFactory.CreatePanelWindow(parentTransform, panelWindowModel, services);
        }
        public void AddContext(ContextWindowModel context)
        {
            IList<ContextWindowModel> _contexts = this.contextObseravable.Get();
            if (_contexts.Find(existingcontext => { return context.ID == existingcontext.ID; }) == null)
            {
                _contexts.Add(context);
                this.contextObseravable.Set(_contexts);
            }
        }

        public void RemoveContext(long modelID)
        {
            this.contextObseravable.Set(this.contextObseravable.Get().Filter(context => { return context.objectID != modelID; }));
        }

        public void RemovePanelsForObject(long modelID)
        {
            if (this.selectedObjectPanels.Get() != null)
            {
                this.selectedObjectPanels.Set(this.selectedObjectPanels.Get().Filter(panel => { return panel.objectID != modelID; }));
            }
        }
    }

}
