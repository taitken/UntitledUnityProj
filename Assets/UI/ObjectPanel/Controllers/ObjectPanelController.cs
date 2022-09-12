using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;
using Zenject;
using UI.Models;
using UI.Services;
using GameControllers.Services;
using System;

namespace UI
{
    public class ObjectPanelController : MonoBehaviour2
    {
        private IUiPanelService uiPanelService;
        private IList<BasePanel> panels = new List<BasePanel>();
        private IList<IBaseService> services;
        [Inject]
        public void Construct(IUiPanelService _contextService, 
                              IItemObjectService _itemService,  
                              IBuildingService _buildingService, 
                              IUnitOrderService _unitOrderService, 
                              ICropService _cropService)
        {
            this.services = new List<IBaseService>();
            this.uiPanelService = _contextService;
            this.uiPanelService.selectedObjectPanels.SubscribeQuietly(this, this.OnObjectSelected);
            this.services.Add(_itemService);
            this.services.Add(_buildingService);
            this.services.Add(_unitOrderService);
            this.services.Add(_cropService);
        }
        // Start is called before the first frame update
        void Start()
        {

        }

        private void OnObjectSelected(IList<BasePanelModel> panels)
        {
            this.panels.DestroyAll();
            if (panels != null)
            {
                panels.ForEach(panel =>
                {
                    BasePanel newPanel = this.uiPanelService.GetPanelAssetFactory().CreatePanelWindow(this.GetComponent<RectTransform>(), panel, this.services);
                    this.panels.Add(newPanel);
                });

            }
        }
    }
}