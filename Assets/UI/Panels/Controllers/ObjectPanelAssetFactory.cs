using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using UI.Models;
using GameControllers.Services;
using System.Linq;
using System;

namespace UI
{
    public class ObjectPanelAssetFactory : MonoBehaviour2
    {
        public List<BasePanel> panelPrefabs;
        public BasePanel CreatePanelWindow(RectTransform parentTransform, BasePanelModel panelWindowModel, IList<IBaseService> services)
        {
            if (this.panelPrefabs.Count > (int)panelWindowModel.panelType)
            {
                BasePanel newPanel = Instantiate(this.panelPrefabs[(int)panelWindowModel.panelType]);
                newPanel.InjectServices(services);
                newPanel.Construct(panelWindowModel);
                newPanel.GetComponent<RectTransform>().SetParent(parentTransform);
                switch (panelWindowModel.panelType)
                {
                    case ePanelTypes.ObjectInfo:
                        newPanel.GetComponent<RectTransform>().anchoredPosition = new Vector3(-190, 270);
                        break;
                    case ePanelTypes.RecipeSelector:
                        newPanel.GetComponent<RectTransform>().anchoredPosition = new Vector3(-413, 270);
                        break;
                    case ePanelTypes.SeedSelector:
                        newPanel.GetComponent<RectTransform>().anchoredPosition = new Vector3(-413, 270);
                        break;
                    case ePanelTypes.BuildingSelector:
                        newPanel.GetComponent<RectTransform>().anchoredPosition = new Vector3(180, 270);
                        break;
                    case ePanelTypes.RoomInfo:
                        newPanel.GetComponent<RectTransform>().anchoredPosition = new Vector3(-190, 270);
                        break;
                }
                return newPanel;
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