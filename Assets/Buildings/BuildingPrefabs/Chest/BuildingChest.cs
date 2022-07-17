using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Building.Models;
using UtilityClasses;
using UI.Models;

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

        public override void OnMouseEnter()
        {
            List<string> newContext = new List<string>();
            newContext.Add(this.buildingObjectModel.mass.ToString() + " " + LocalisationDict.weight);
            newContext.Add("Can store other items");
            this.contextService.AddContext(new ContextWindowModel(this.buildingObjectModel.ID, this.buildingObjectModel.buildingType.ToString(), newContext));
        }

        public class Factory : PlaceholderFactory<BuildingObjectModel, BuildingChest>
        {
        }
    }
}