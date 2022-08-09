
using System.Collections.Generic;
using UnityEngine;
using GameControllers.Services;
using Zenject;
using GameControllers.Models;
using UI.Services;
using Characters;
using Unit.Models;

namespace Environment
{
    public class CharacterLayer : MonoBehaviourLayer
    {
        private IUnitOrderService orderService;
        private IUnitService unitService;
        private MouseActionModel mouseAction;
        private PlayerController.Factory characterFactory;
        public IList<WorldCharacter> worldCharacters = new List<WorldCharacter>();
        private IList<UnitModel> unitModels
        {
            get
            {
                return this.worldCharacters.Map(character => { return character.unitModel; });
            }
        }

        [Inject]
        public void Construct(IUnitOrderService _orderService,
                              IUnitService _unitService,
                              LayerCollider.Factory _layerColliderFactory,
                              PlayerController.Factory _characterFactory)
        {
            this.InitiliseMonoLayer(_layerColliderFactory, new Vector2(MonoBehaviourLayer.MAP_WIDTH, MonoBehaviourLayer.MAP_HEIGHT), "CharacterLayer");
            this.orderService = _orderService;
            this.unitService = _unitService;
            this.characterFactory = _characterFactory;
        }

        void Start()
        {
            this.unitService.unitObseravable.Subscribe(this, this.HandleUnitModels);
            this.unitService.AddUnit(new UnitModel(.75f, new Vector3(8f, 8f, 0)));
            this.unitService.AddUnit(new UnitModel(.75f, new Vector3(8.05f, 8f, 0)));
            this.unitService.AddUnit(new UnitModel(.75f, new Vector3(8.1f, 8f, 0)));
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

        private WorldCharacter createUnit(UnitModel newUnit)
        {
            WorldCharacter newChar = this.characterFactory.Create(newUnit);
            newChar.transform.position = newUnit.position;
            return newChar;
        }
    }
}