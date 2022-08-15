using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Building;
using Building.Models;
using Zenject;
using Environment.Models;
using UtilityClasses;

namespace GameControllers
{
    public class BuildingAssetController : MonoBehaviour2
    {
        public Texture2D[] wallSpriteSheets;
        public Sprite[][] wallSprites;
        public List<BuildingObject> buildingPrefabs;
        public GameObject buildGhostPrefab;

        public void Initialise()
        {
            this.wallSprites = new Sprite[wallSpriteSheets.Length][];
            this.wallSpriteSheets.ForEach((sheet, index) =>
            {
                this.wallSprites[index] = Resources.LoadAll<Sprite>(sheet.name);
            });
        }

        public GameObject GetBuildingGhostPrefab(eBuildingType buildingType)
        {
            GameObject ghostBuilding = Instantiate(this.buildGhostPrefab.gameObject, default(Vector3), new Quaternion());
            SpriteRenderer sr = ghostBuilding.GetComponent<SpriteRenderer>();
            sr.sprite = this.GetBuildingSprite(buildingType).sprite;
            sr.color = GameColors.AddTransparency(sr.color, 0.6f);
            sr.transform.localScale = this.GetBuildingSprite(buildingType).transform.localScale;
            return ghostBuilding;
        }

        public BuildingObject GetBuildingPrefab(eBuildingType buildingType)
        {
            if (this.buildingPrefabs.Count > (int)buildingType)
            {
                return this.buildingPrefabs[(int)buildingType] as BuildingObject;
            }
            else
            {
                this.ThrowMissingPrefabError(buildingType);
                return null;
            }
        }

        public SpriteRenderer GetBuildingSprite(eBuildingType buildingType)
        {
            if (this.buildingPrefabs.Count > (int)buildingType)
            {
                return this.buildingPrefabs[(int)buildingType].gameObject.GetComponent<SpriteRenderer>();
            }
            else
            {
                this.ThrowMissingPrefabError(buildingType);
                return null;
            }
        }

        public Sprite[] GetWallSpriteSet(eWallTypes wallType)
        {
            if (this.wallSprites.Length > (int)wallType)
            {
                return this.wallSprites[(int)wallType];
            }
            else
            {
                this.ThrowMissingSpriteError(wallType);
                return null;
            }
        }

        private void ThrowMissingSpriteError(eWallTypes wallType)
        {
            Debug.LogException(new System.Exception("Chosen wall type sprite set has not been added to the Wall Asset Controller. Attemped type: " + wallType.ToString()));
        }

        private void ThrowMissingPrefabError(eBuildingType buildingType)
        {
            Debug.LogException(new System.Exception("Chosen building prefab has not been added to the Building Asset Controller. Attemped type: " + buildingType.ToString()));
        }
    }
}