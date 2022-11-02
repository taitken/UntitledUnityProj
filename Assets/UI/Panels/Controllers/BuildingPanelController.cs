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
    public class BuildingPanelController : MonoBehaviour2
    {
        private IUiPanelService uiPanelService;
        private IList<BasePanel> panels = new List<BasePanel>();
        private IList<IBaseService> services;
        [Inject]
        public void Construct(IUiPanelService _contextService, 
                              IItemObjectService _itemService,  
                              IBuildingService _buildingService, 
                              IUnitOrderService _unitOrderService)
        {
            this.services = new List<IBaseService>();
            this.uiPanelService = _contextService;
            this.uiPanelService.selectedBuildingPanels.SubscribeQuietly(this, this.OnBuildingsSelected);
            this.services.Add(_itemService);
            this.services.Add(_buildingService);
            this.services.Add(_unitOrderService);
        }
        // Start is called before the first frame update
        void Start()
        {

        }

        private void OnBuildingsSelected(IList<BasePanelModel> panels)
        {
            this.panels.DestroyAll();
            if (panels != null)
            {
                panels.ForEach(panel =>
                {
                    BasePanel newPanel = this.uiPanelService.CreatePanelWindow(this.GetComponent<RectTransform>(), panel, this.services);
                    this.panels.Add(newPanel);
                });

            }
        }
    }
}