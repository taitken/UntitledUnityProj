using System.Collections.Generic;
using UnityEngine;
using Building.Models;
using Extensions;
using UnityEngine.Tilemaps;
using UI.Services;
using UtilityClasses;
using UI.Models;
using GameControllers.Services;
using Zenject;

namespace Building
{
    public class BuildSiteObject : MonoBehaviour2
    {
        public BuildSiteModel buildSiteModel { get; set; }
        protected IContextWindowService contextService { get; set; }

        [Inject]
        public void Construct(IContextWindowService _contextService,
                                BuildSiteModel _buildSiteModel,
                                IEnvironmentService _environmentService)
        {
            this.buildSiteModel = _buildSiteModel;
            this.transform.position = _environmentService.CellToLocal(_buildSiteModel.position);
            this.contextService = _contextService;
        }

        public override void OnMouseEnter()
        {
            this.contextService.AddContext(new ContextWindowModel(this.buildSiteModel.ID, this.GenerateContextWindowTitle(), this.GenerateContextWindowBody()));
        }

        public override void OnMouseExit()
        {
            this.contextService.RemoveContext(this.buildSiteModel.ID);
        }

        protected string GenerateContextWindowTitle()
        {
            return this.buildSiteModel.buildingType.ToString().FirstCharToUpper() + " (construction)";
        }

        protected virtual List<string> GenerateContextWindowBody()
        {
            List<string> newContext = new List<string>();
            newContext.Add("Required: " + this.buildSiteModel.buildingModel.requiredItems[0].itemType.ToString() + ":" + ((int)this.buildSiteModel.buildingModel.requiredItems[0].mass).ToString() + " " + LocalisationDict.mass);
            newContext.Add(((int)this.buildSiteModel.supplyCurrent).ToString() + " " + LocalisationDict.mass);
            return newContext;
        }

        public class Factory : PlaceholderFactory<BuildSiteModel, BuildSiteObject>
        {
        }
    }
}
