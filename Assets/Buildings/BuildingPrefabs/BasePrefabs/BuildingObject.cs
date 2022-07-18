using System.Collections.Generic;
using UnityEngine;
using Building.Models;
using Extensions;
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

        public virtual void Initialise(IContextWindowService _contextService,
                                        BuildingObjectModel _buildingObjectModel,
                                        Tilemap tilemap)
        {
            this.buildingObjectModel = _buildingObjectModel;
            this.transform.position = tilemap.CellToLocal(_buildingObjectModel.position);
            this.contextService = _contextService;
        }

        public override void OnMouseEnter()
        {

            this.contextService.AddContext(new ContextWindowModel(this.buildingObjectModel.ID, this.GenerateContextWindowTitle(), this.GenerateContextWindowBody()));
        }

        public override void OnMouseExit()
        {
            this.contextService.RemoveContext(this.buildingObjectModel.ID);
        }

        protected string GenerateContextWindowTitle()
        {
            return this.buildingObjectModel.buildingType.ToString().FirstCharToUpper();
        }

        protected virtual List<string> GenerateContextWindowBody()
        {
            List<string> newContext = new List<string>();
            newContext.Add("Is a building");
            return newContext;
        }

    }
}
