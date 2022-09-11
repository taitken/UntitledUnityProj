using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Building;
using Building.Models;
using Zenject;
using Environment.Models;
using Item.Models;

namespace GameControllers
{
    public class ItemAssetController : MonoBehaviour2
    {
        public List<Sprite> itemSprites;

        [Inject]
        public void Construct()
        {

        }

        public Sprite GetItemSprite(eItemType itemType)
        {
            if (this.itemSprites.Count > (int)itemType)
            {
                return this.itemSprites[(int)itemType];
            }
            else
            {
                this.ThrowMissingItemError(itemType);
                return null;
            }
        }

        private void ThrowMissingItemError(eItemType itemType)
        {
            Debug.LogException(new System.Exception("Chosen item prefab has not been added to the Item Asset Controller. Attemped type: " + itemType.ToString()));
        }
    }
}