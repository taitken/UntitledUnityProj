using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameControllers.Services;
using GameControllers.Models;
using Building.Models;
using Zenject;
using UnityEngine.Tilemaps;
using UI.Services;
using UtilityClasses;
using UI.Models;

namespace Building
{
    public class BuildingObject : MonoBehaviour2
    {
        public BuildingObjectModel buildingObjectModel { get; set; }
        protected IContextWindowService contextService { get; set; }
        [Inject]
        public void Construct(BuildingObjectModel _buildingObjectModel,
                                IContextWindowService _contextService)
        {
            this.buildingObjectModel = _buildingObjectModel;
            this.contextService = _contextService;
        }

        public void Initialise(BuildingObjectModel _buildingObjectModel, Tilemap tilemap)
        {
            this.buildingObjectModel = _buildingObjectModel;
            this.transform.position = tilemap.CellToLocal(_buildingObjectModel.position);
        }


        public override void OnMouseEnter()
        {
            List<string> newContext = new List<string>();
            newContext.Add(this.buildingObjectModel.mass.ToString() + " " + LocalisationDict.weight);
            newContext.Add("Is a building");
            this.contextService.AddContext(new ContextWindowModel(this.buildingObjectModel.ID, this.buildingObjectModel.buildingType.ToString(), newContext));
        }

        public override void OnMouseExit()
        {
            this.contextService.RemoveContext(this.buildingObjectModel.ID);
        }

    }
}
