using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameControllers.Services;
using GameControllers.Models;
using Building.Models;
using Zenject;
using UnityEngine.Tilemaps;

namespace Building
{
    public class BuildingObject : MonoBehaviour2
    {
        public BuildingObjectModel buildingObjectModel {get;set;}
        [Inject]
        public void Construct(BuildingObjectModel _buildingObjectModel)
        {
            this.buildingObjectModel = _buildingObjectModel;
        }

        public void Initialise(BuildingObjectModel _buildingObjectModel, Tilemap tilemap)
        {
            this.buildingObjectModel = _buildingObjectModel;
            this.transform.position = tilemap.CellToLocal(_buildingObjectModel.position);
        }
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

    }
}
