using System.Collections.Generic;
using UnityEngine;
using Building.Models;
using Extensions;
using UnityEngine.Tilemaps;
using UI.Services;
using UtilityClasses;
using UI.Models;
using GameControllers.Services;
using Item.Models;

namespace Building
{
    public abstract class BuildingObject : MonoBehaviour2
    {
        public BuildingObjectModel buildingObjectModel { get; set; }
        protected IUnitOrderService unitOrderService { get; set; }
        protected IItemObjectService itemService {get;set;}
        protected IUiPanelService contextService { get; set; }

        public void Initialise(IUiPanelService _contextService,
                                        BuildingObjectModel _buildingObjectModel,
                                        IEnvironmentService _environmentService,
                                        IItemObjectService _itemObjectService,
                                        IUnitOrderService _orderService)
        {
            this.buildingObjectModel = _buildingObjectModel;
            this.unitOrderService = _orderService;
            this.itemService = _itemObjectService;
            this.SetMultiTilePosition(_environmentService.CellToLocal(_buildingObjectModel.position));
            if(this.buildingObjectModel.buildingType != eBuildingType.FloorTile) this.UpdateBuildingBounds();
            this.contextService = _contextService;


            this.OnCreation();
        }

        protected abstract void OnCreation();
        

        public override void OnMouseEnter()
        {

            this.contextService.AddContext(new ObjectContextWindowModel(this.buildingObjectModel.ID, this.GenerateContextWindowTitle(), this.GenerateContextWindowBody()));
        }

        public override void OnMouseExit()
        {
            this.contextService.RemoveContext(this.buildingObjectModel.ID);
        }

        protected string GenerateContextWindowTitle()
        {
            return this.buildingObjectModel.buildingType.ToString().FirstCharToUpper();
        }

        protected void UpdateBuildingBounds()
        {
            SpriteRenderer sr = this.GetComponent<SpriteRenderer>();
            BoxCollider2D bc = this.GetComponent<BoxCollider2D>();
            Bounds bounds = new Bounds(new Vector3(this.buildingObjectModel.size.x * (float)IEnvironmentService.TILE_WIDTH_PIXELS / 2, this.buildingObjectModel.size.y * (float)IEnvironmentService.TILE_WIDTH_PIXELS / 2),
                                        new Vector3(this.buildingObjectModel.size.x * IEnvironmentService.TILE_WIDTH_PIXELS, this.buildingObjectModel.size.y * IEnvironmentService.TILE_WIDTH_PIXELS));
            bc.offset = new Vector3(((sr.bounds.size.x / IEnvironmentService.TILE_WIDTH_PIXELS) - this.buildingObjectModel.size.x) * -IEnvironmentService.TILE_WIDTH_PIXELS / 2,
                                    ((sr.bounds.size.y / IEnvironmentService.TILE_WIDTH_PIXELS) - this.buildingObjectModel.size.y) * -IEnvironmentService.TILE_WIDTH_PIXELS / 2);
            bc.size = bounds.size;
        }

        protected virtual List<string> GenerateContextWindowBody()
        {
            List<string> newContext = new List<string>();
            return newContext;
        }

    }
}
