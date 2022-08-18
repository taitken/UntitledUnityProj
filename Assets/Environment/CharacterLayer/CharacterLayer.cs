
using System.Collections.Generic;
using UnityEngine;
using GameControllers.Services;
using Zenject;
using GameControllers.Models;
using UI.Services;
using Characters;
using Unit.Models;
using Building.Models;
using Environment.Models;
using System.Linq;
using System;

namespace Environment
{
    public class CharacterLayer : MonoBehaviourLayer
    {
        private IUnitOrderService orderService;
        private IUnitService unitService;
        private IBuildingService buildingService;
        private IEnvironmentService envService;
        private MouseActionModel mouseAction;
        private PlayerController.Factory characterFactory;
        public IList<WorldCharacter> worldCharacters = new List<WorldCharacter>();
        private IList<UnitModel> unitModels { get { return this.worldCharacters.Map(character => { return character.unitModel; }); } }

        [Inject]
        public void Construct(IUnitOrderService _orderService,
                              IUnitService _unitService,
                              IEnvironmentService _envService,
                              IBuildingService _buildingService,
                              LayerCollider.Factory _layerColliderFactory,
                              PlayerController.Factory _characterFactory)
        {
            this.InitiliseMonoLayer(_layerColliderFactory, new Vector2(MonoBehaviourLayer.MAP_WIDTH, MonoBehaviourLayer.MAP_HEIGHT), "CharacterLayer");
            this.orderService = _orderService;
            this.unitService = _unitService;
            this.envService = _envService;
            this.characterFactory = _characterFactory;
            this.buildingService = _buildingService;
            this.buildingService.SubscribeToNewBuildingTrigger(this, this.HandleNewBuilding);
        }

        void Start()
        {
            this.unitService.unitObseravable.Subscribe(this, this.HandleUnitModels);
            this.unitService.AddUnit(new UnitModel(.75f, new Vector3(8f, 8f, 0), this.envService.LocalToCell(new Vector3(8f, 8f, 0))));
            this.unitService.AddUnit(new UnitModel(.75f, new Vector3(8.05f, 8f, 0), this.envService.LocalToCell(new Vector3(8f, 8f, 0))));
            this.unitService.AddUnit(new UnitModel(.75f, new Vector3(8.1f, 8f, 0), this.envService.LocalToCell(new Vector3(8f, 8f, 0))));
            this.unitService.AddUnit(new UnitModel(.75f, new Vector3(8.2f, 8f, 0), this.envService.LocalToCell(new Vector3(8f, 8f, 0))));
            this.unitService.AddUnit(new UnitModel(.75f, new Vector3(8.3f, 8f, 0), this.envService.LocalToCell(new Vector3(8f, 8f, 0))));
            this.unitService.AddUnit(new UnitModel(.75f, new Vector3(8.4f, 8f, 0), this.envService.LocalToCell(new Vector3(8f, 8f, 0))));
        }
        
        // Update is called once per frame
        void Update()
        {

        }

        void HandleUnitModels(IList<UnitModel> updatedUnitModels)
        {
            IList<UnitModel> newModels = updatedUnitModels.GetNewModels(this.unitModels);
            IList<UnitModel> removedModels = updatedUnitModels.GetRemovedModels(unitModels);
            newModels.ForEach(newModel =>
            {
                this.worldCharacters.Add(this.createUnit(newModel));
            });
            removedModels.ForEach(removedModels =>
            {
                WorldCharacter worldCharacterToRemove = this.worldCharacters.Find(character => { return character.unitModel.ID == removedModels.ID; });
                this.worldCharacters.Remove(worldCharacterToRemove);
                worldCharacterToRemove.Destroy();
            });
        }

        private void HandleNewBuilding(BuildingObjectModel newBuilding)
        {
            if (newBuilding is WallBuildingModel)
            {
                MineableObjectModel[,] _mineableBlocks = this.envService.mineableObjects.Get();
                BuildingObjectModel[,] _walls = new BuildingObjectModel[MonoBehaviourLayer.MAP_WIDTH, MonoBehaviourLayer.MAP_HEIGHT];
                this.buildingService.buildingObseravable.Get().Filter(building => { return building is WallBuildingModel; }).ForEach(wall => { _walls[wall.position.x, wall.position.y] = wall; });
                this.MoveObjectOffInvalidPosition(this.worldCharacters.Cast<MonoBaseObject>().ToList(), newBuilding.position, _walls, _mineableBlocks);
            }
        }

        private WorldCharacter createUnit(UnitModel newUnit)
        {
            WorldCharacter newChar = this.characterFactory.Create(newUnit);
            newChar.transform.position = newUnit.localPosition;
            return newChar;
        }
    }
}