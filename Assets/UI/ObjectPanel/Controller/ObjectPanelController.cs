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
        private IList<ObjectPanel> panels = new List<ObjectPanel>();
        private IItemObjectService itemService;
        [Inject]
        public void Construct(IUiPanelService _contextService, IItemObjectService _itemService)
        {
            this.uiPanelService = _contextService;
            this.itemService = _itemService;
            this.uiPanelService.selectedObject.SubscribeQuietly(this, this.OnObjectSelected);

        }
        // Start is called before the first frame update
        void Start()
        {

        }

        private void OnObjectSelected(PanelModel selectedObject)
        {
            this.panels.DestroyAll();
            if (selectedObject != null)
            {
                ObjectPanel newPanel = this.uiPanelService.panelAssetFactory.CreatePanelWindow(this.GetComponent<RectTransform>(), selectedObject, this.itemService);
                this.panels.Add(newPanel);
            }
        }
    }
}