using System;
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
        public List<BuildingObject> buildingPrefabInput;
        private BuildingObject[] buildingPrefabs;
        public GameObject buildGhostPrefab;

        public void Initialise()
        {
            BuildingObject[] buildings = new BuildingObject[Enum.GetValues(typeof(eBuildingType)).Length];
            this.buildingPrefabs = this.BuildPrefabArray();
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
            if (this.buildingPrefabs.Length > (int)buildingType)
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
            if (this.buildingPrefabs.Length > (int)buildingType)
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
                this.ThrowMissingSpriteError(wallType.ToString());
                return null;
            }
        }

        public BuildingObject[] BuildPrefabArray()
        {
            int buildingCount = Enum.GetValues(typeof(eBuildingType)).Length;
            BuildingObject[] buildings = new BuildingObject[buildingCount];
            for (int i = 1; i < buildingCount; i++)
            {
                buildings[i] = this.buildingPrefabInput.Find(prefab => { return prefab != null ? (int)prefab.buildingType == i : false; });
                if (buildings[i] == null)
                {
                    this.ThrowMissingPrefabError((eBuildingType)i);
                }
            }
            return buildings;
        }

        private void ThrowMissingSpriteError(string spriteName)
        {
            Debug.LogException(new System.Exception("Chosen wall type sprite set has not been added to the Wall Asset Controller. Attemped type: " + spriteName));
        }

        private void ThrowMissingPrefabError(eBuildingType buildingType)
        {
            Debug.LogException(new System.Exception("Chosen building prefab has not been added to the Building Asset Controller. Attemped type: " + buildingType.ToString()));
        }
    }
}