using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using GameControllers.Services;
using Characters;
using Building.Models;
using Zenject;
using Environment.Models;

namespace GameControllers
{
    public class BuildingAssetController : MonoBehaviour2
    {

        public List<MonoBehaviour2> buildingPrefabs;

        [Inject]
        public void Construct()
        {

        }

        public SpriteRenderer GetBuildingSprite(eBuildingType buildingType)
        {
            if(this.buildingPrefabs.Count > (int)buildingType)
            {
                return this.buildingPrefabs[(int)buildingType].gameObject.GetComponent<SpriteRenderer>();
            } else
            {
                Debug.LogException(new System.Exception("Chosen building prefab has not been added to the Building Asset Controller. Attemped type: " + buildingType.ToString()));
                return null;
            }
        }
    }
}