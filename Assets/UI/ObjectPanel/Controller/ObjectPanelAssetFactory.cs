using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using UI.Models;
using GameControllers.Services;
using System.Linq;

namespace UI
{
    public class ObjectPanelAssetFactory : MonoBehaviour2
    {
        public List<ObjectPanel> panelPrefabs;

        public ObjectPanel CreatePanelWindow(RectTransform parentTransform, PanelModel panelWindowModel, IItemObjectService itemService)
        {
            if (this.panelPrefabs.Count > (int)panelWindowModel.panelType)
            {
                ObjectPanel newPanel = Instantiate(this.panelPrefabs[(int)panelWindowModel.panelType]);
                newPanel.Construct(panelWindowModel);
                newPanel.GetComponent<RectTransform>().SetParent(parentTransform);
                switch (panelWindowModel.panelType)
                {
                    case ePanelTypes.ObjectInfo:
                        newPanel.GetComponent<RectTransform>().anchoredPosition = new Vector3(-150, 230);
                        break;
                }
                return newPanel as ObjectPanel;
            }
            else
            {
                this.ThrowMissingPrefabError(panelWindowModel.panelType);
                return null;
            }
        }

        private void ThrowMissingPrefabError(ePanelTypes panelType)
        {
            Debug.LogException(new System.Exception("Chosen context prefab has not been added to the Context Asset Factory. Attemped type: " + panelType.ToString()));
        }
    }
}