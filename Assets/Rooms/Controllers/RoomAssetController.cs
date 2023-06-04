
using UnityEngine;
using Building.Models;

namespace GameControllers
{
    public class RoomAssetController : MonoBehaviour2
    {
        public GameObject tileHighlightPrefab;

        public void Initialise()
        {

        }

        public GameObject GetTileHighlightPrefab()
        {
            if(this.tileHighlightPrefab == null)
            {
                Debug.LogException(new System.Exception("Tile highlight prefab has not been added to the Room Asset Controller"));
                return null;
            }
            return this.tileHighlightPrefab;
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