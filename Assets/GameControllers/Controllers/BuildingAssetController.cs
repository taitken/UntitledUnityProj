using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Building;
using Building.Models;
using Zenject;
using Environment.Models;

namespace GameControllers
{
    public class BuildingAssetController : MonoBehaviour2
    {

        public List<BuildingObject> buildingPrefabs;

        [Inject]
        public void Construct()
        {

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

        private void ThrowMissingPrefabError(eBuildingType buildingType)
        {
                Debug.LogException(new System.Exception("Chosen building prefab has not been added to the Building Asset Controller. Attemped type: " + buildingType.ToString()));
        }
    }
}