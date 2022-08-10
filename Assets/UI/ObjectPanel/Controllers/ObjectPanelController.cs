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
        private IItemObjectService itemService;
        [Inject]
        public void Construct(IUiPanelService _contextService, IItemObjectService _itemService)
        {
            this.uiPanelService = _contextService;
            this.itemService = _itemService;
            this.uiPanelService.selectedObjectPanels.SubscribeQuietly(this, this.OnObjectSelected);
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
                    BasePanel newPanel = this.uiPanelService.GetPanelAssetFactory().CreatePanelWindow(this.GetComponent<RectTransform>(), panel, this.itemService);
                    this.panels.Add(newPanel);
                });

            }
        }
    }
}