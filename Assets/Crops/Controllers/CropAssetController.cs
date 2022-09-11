using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Building;
using Building.Models;
using Zenject;
using Environment.Models;
using UtilityClasses;
using Crops;

namespace GameControllers
{
    public class CropAssetController : MonoBehaviour2
    {
        public Texture2D[] seedSpriteSheets;
        public Sprite[][] seedSprites;

        public void Initialise()
        {
            this.seedSprites = new Sprite[seedSpriteSheets.Length][];
            this.seedSpriteSheets.ForEach((sheet, index) =>
            {
                this.seedSprites[index] = Resources.LoadAll<Sprite>(sheet.name);
            });
        }

        // public BuildingObject GetBuildingPrefab(eBuildingType buildingType)
        // {
        //     if (this.buildingPrefabs.Count > (int)buildingType)
        //     {
        //         return this.buildingPrefabs[(int)buildingType] as BuildingObject;
        //     }
        //     else
        //     {
        //         this.ThrowMissingPrefabError(buildingType);
        //         return null;
        //     }
        // }

        // public SpriteRenderer GetBuildingSprite(eBuildingType buildingType)
        // {
        //     if (this.buildingPrefabs.Count > (int)buildingType)
        //     {
        //         return this.buildingPrefabs[(int)buildingType].gameObject.GetComponent<SpriteRenderer>();
        //     }
        //     else
        //     {
        //         this.ThrowMissingPrefabError(buildingType);
        //         return null;
        //     }
        // }

        // public Sprite[] GetWallSpriteSet(eWallTypes wallType)
        // {
        //     if (this.wallSprites.Length > (int)wallType)
        //     {
        //         return this.wallSprites[(int)wallType];
        //     }
        //     else
        //     {
        //         this.ThrowMissingSpriteError(wallType);
        //         return null;
        //     }
        // }

        // private void ThrowMissingSpriteError(eWallTypes wallType)
        // {
        //     Debug.LogException(new System.Exception("Chosen wall type sprite set has not been added to the Wall Asset Controller. Attemped type: " + wallType.ToString()));
        // }

        // private void ThrowMissingPrefabError(eBuildingType buildingType)
        // {
        //     Debug.LogException(new System.Exception("Chosen building prefab has not been added to the Building Asset Controller. Attemped type: " + buildingType.ToString()));
        // }
    }
}