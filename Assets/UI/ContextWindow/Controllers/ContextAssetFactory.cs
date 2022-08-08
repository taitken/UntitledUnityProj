using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using UI.Models;
using GameControllers.Services;
using System.Linq;
using Item.Models;

namespace UI
{
    public class ContextAssetFactory : MonoBehaviour2
    {
        public List<ContextWindow> contextPrefabs;

        public ContextWindow CreateContextWindow(RectTransform parentTransform, ContextWindowModel contextWindowModel, IItemObjectService itemService)
        {
            if (this.contextPrefabs.Count > (int)contextWindowModel.contextType)
            {
                ContextWindow newWindow = Instantiate(this.contextPrefabs[(int)contextWindowModel.contextType], default(Vector3), new Quaternion());
                newWindow.Construct(contextWindowModel);
                newWindow.GetComponent<RectTransform>().SetParent(parentTransform);
                switch (contextWindowModel.contextType)
                {
                    case eContextTypes.Object:
                        break;
                    case eContextTypes.ProductionBuilding:
                        ProductionBuildingContextWindow productBuildingCW = newWindow as ProductionBuildingContextWindow;
                        ProductionBuildingContextWindowModel productCWModel = contextWindowModel as ProductionBuildingContextWindowModel;
                        IList<(eItemType, Sprite)> spriteList = productCWModel.productionBuildingModel.inputs.Map(input => { return (input.itemType, itemService.GetItemSprite(input.itemType)); });
                        spriteList.AddRange(productCWModel.productionBuildingModel.outputs.Map(output => { return (output.itemType, itemService.GetItemSprite(output.itemType)); }));
                        productBuildingCW.SetItemSprites(spriteList.Distinct().ToList());
                        break;
                }
                return newWindow as ContextWindow;
            }
            else
            {
                this.ThrowMissingPrefabError(contextWindowModel.contextType);
                return null;
            }
        }

        private void ThrowMissingPrefabError(eContextTypes contextType)
        {
            Debug.LogException(new System.Exception("Chosen context prefab has not been added to the Context Asset Factory. Attemped type: " + contextType.ToString()));
        }
    }
}