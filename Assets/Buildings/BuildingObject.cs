using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameControllers.Services;
using GameControllers.Models;
using Building.Models;
using Zenject;

namespace Building
{
    public class BuildingObject : MonoBehaviour2
    {
        BuildingObjectModel buildingObject;
        [Inject]
        public void Construct(BuildingObjectModel _buildingObjectModel)
        {
            this.buildingObject = _buildingObjectModel;
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
