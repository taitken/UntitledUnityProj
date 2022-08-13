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
using GameControllers.Models;
using Item.Models;

namespace Building
{
    public class BuildSiteObject : MonoBehaviour2
    {
        public IUnitOrderService orderService { get; set; }
        private IBuildingService buildingService { get; set; }
        private IItemObjectService itemService { get; set; }
        private IList<GameObject> spriteObjects { get; set; } = new List<GameObject>();
        public BuildSiteModel buildSiteModel { get; set; }
        private bool cancelled { get; set; }
        protected IUiPanelService contextService { get; set; }

        [Inject]
        public void Construct(IUiPanelService _contextService,
                                BuildSiteModel _buildSiteModel,
                                IEnvironmentService _environmentService,
                                IBuildingService _buildingService,
                                IItemObjectService _itemService,
                                IUnitOrderService _orderService)
        {
            this.transform.position = _environmentService.CellToLocal(_buildSiteModel.position);
            this.buildSiteModel = _buildSiteModel;
            this.contextService = _contextService;
            this.itemService = _itemService;
            this.orderService = _orderService;
            this.buildingService = _buildingService;
            this.InitaliseSprites();
        }

        private void InitaliseSprites()
        {
            foreach (Transform child in transform)
            {
                this.spriteObjects.Add(child.gameObject);
            }
            for (int x = 0; x < this.buildSiteModel.buildingModel.size.x; x++)
            {
                for (int y = 0; y < this.buildSiteModel.buildingModel.size.y; y++)
                {
                    if (x + y > 0)
                    {
                        GameObject gameObject = Instantiate(spriteObjects[0], new Vector3(0,0), default(Quaternion));
                        gameObject.transform.SetParent(this.transform);
                        gameObject.transform.localPosition = new Vector3(x * IEnvironmentService.TILE_WIDTH_PIXELS, y * IEnvironmentService.TILE_WIDTH_PIXELS);
                        this.spriteObjects.Add(gameObject);
                    }
                }
            }
            this.UpdateBoxColliderToFitChildren();
        }

        public override void OnMouseEnter()
        {
            this.contextService.AddContext(new ObjectContextWindowModel(this.buildSiteModel.ID, this.GenerateContextWindowTitle(), this.GenerateContextWindowBody()));
        }

        public override void OnMouseExit()
        {
            this.contextService.RemoveContext(this.buildSiteModel.ID);
        }

        public override void OnClickedByUser()
        {
            if (this.orderService.mouseAction.Get().mouseType == eMouseAction.Cancel)
            {
                UnitOrderModel order = this.orderService.orders.Get().Find(order => { return order.coordinates == this.buildSiteModel.position; });
                this.cancelled = true;
                if (order != null) this.orderService.RemoveOrder(order.ID);
                this.buildingService.RemoveBuildSite(this.buildSiteModel.ID);
            }
        }

        protected override void BeforeDeath()
        {
            this.contextService.RemoveContext(this.buildSiteModel.ID);
            if (this.cancelled)
            {
                this.buildSiteModel.suppliedItems.ForEach(item =>
                {
                    item.itemState = ItemObjectModel.eItemState.OnGround;
                    this.itemService.onItemPickupOrDropTrigger.Set(item);
                });
            }
            else
            {
                this.buildSiteModel.suppliedItems.ForEach(item =>
                {
                    this.itemService.RemoveItem(item.ID);
                });
            }
        }

        protected string GenerateContextWindowTitle()
        {
            return this.buildSiteModel.buildingType.ToString().FirstCharToUpper() + " (construction)";
        }

        protected virtual List<string> GenerateContextWindowBody()
        {
            List<string> newContext = new List<string>();
            newContext.Add("Required: " + this.buildSiteModel.buildingModel.requiredItems[0].itemType.ToString() + ":" + LocalisationDict.GetMassString(this.buildSiteModel.buildingModel.requiredItems[0].mass));
            newContext.Add(LocalisationDict.GetMassString(this.buildSiteModel.supplyCurrent));
            return newContext;
        }

        public class Factory : PlaceholderFactory<BuildSiteModel, BuildSiteObject>
        {
        }
    }
}
