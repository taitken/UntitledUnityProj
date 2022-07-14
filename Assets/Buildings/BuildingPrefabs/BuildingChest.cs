using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Building.Models;
namespace Building
{
    public class BuildingChest : BuildingObject
    {
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public class Factory : PlaceholderFactory<BuildingObjectModel, BuildingChest>
        {
        }
    }
}