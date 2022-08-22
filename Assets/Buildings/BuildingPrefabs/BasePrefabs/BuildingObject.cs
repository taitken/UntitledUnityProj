using System.Collections.Generic;
using UnityEngine;
using Building.Models;
using Extensions;
using UI.Services;
using UI.Models;
using GameControllers.Services;
using GameControllers.Models;
using System;

namespace Building
{
    public abstract class BuildingObject : MonoBaseObject
    {
        public BuildingObjectModel buildingObjectModel { get; set; }
        protected IUnitOrderService unitOrderService { get; set; }
        protected IItemObjectService itemService { get; set; }
        protected IUiPanelService uiPanelService { get; set; }
        protected IBuildingService buildingService { get; set; }
        public void Initialise(IUiPanelService _uiPanelService,
                                        BuildingObjectModel _buildingObjectModel,
                                        IEnvironmentService _environmentService,
                                        IItemObjectService _itemObjectService,
                                        IBuildingService _buildingService,
                                        IUnitOrderService _orderService)
        {
            this.buildingObjectModel = _buildingObjectModel;
            this.unitOrderService = _orderService;
            this.itemService = _itemObjectService;
            this.buildingService = _buildingService;
            this.SetMultiTilePosition(_environmentService.CellToLocal(_buildingObjectModel.position));
            if (this.buildingObjectModel.buildingType != eBuildingType.FloorTile) this.UpdateBuildingBounds();
            this.uiPanelService = _uiPanelService;
            this.OnCreation();
        }

        public override void OnSelect()
        {
            IList<BasePanelModel> panels = new List<BasePanelModel>();
            panels.Add(new ObjectPanelModel(this.buildingObjectModel.ID, this.buildingObjectModel.buildingType.ToString(), this.buildingObjectModel));
            this.uiPanelService.selectedObjectPanels.Set(panels);
        }

        protected virtual void OnCreation()
        {

        }

        public override BaseObjectModel GetBaseObjectModel()
        {
            return this.buildingObjectModel;
        }

        public override void OnClickedByUser()
        {
            if (this.unitOrderService.mouseAction.Get().mouseType == eMouseAction.Deconstruct)
            {
                this.unitOrderService.AddOrder(new DeconstructOrderModel(this.buildingObjectModel.position, this.buildingObjectModel, true));
            }
        }


        public override void OnMouseEnter()
        {

            this.uiPanelService.AddContext(new ObjectContextWindowModel(this.buildingObjectModel.ID, this.GenerateContextWindowTitle(), this.GenerateContextWindowBody()));
        }

        public override void OnMouseExit()
        {
            this.uiPanelService.RemoveContext(this.buildingObjectModel.ID);
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
            newContext.Add("Position: " + this.buildingObjectModel.position.ToString());
            return newContext;
        }

    }
}
