using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using GameControllers.Services;
using Characters;
using GameControllers.Models;
using Zenject;
using Environment.Models;

namespace Environment
{
    public class UnitController : MonoBehaviour2
    {
        private WorldCharacter.Factory characterFactory;
        private IUnitOrderService orderService;
        private IUnitService unitService;
        public IList<WorldCharacter> worldCharacters = new List<WorldCharacter>();

        public IList<UnitModel> unitModels
        {
            get
            {
                return worldCharacters.Map(character => { return character.unitModel; });
            }
        }

        [Inject]
        public void Construct(IUnitOrderService _orderService,
                              IUnitService _unitService,
                              WorldCharacter.Factory _characterFactory)
        {
            this.orderService = _orderService;
            this.unitService = _unitService;
            this.characterFactory = _characterFactory;
        }
        // Start is called before the first frame update
        void Start()
        {
            this.subscriptions.Add(this.unitService.unitSubscribable.Subscribe(updatedUnitModels =>
            {
                IList<UnitModel> newModels = updatedUnitModels.GetNewModels(this.unitModels);
                newModels.ForEach(newModel =>
                {
                    this.worldCharacters.Add(this.createUnit(newModel));
                });
                IList<UnitModel> removedModels = updatedUnitModels.GetRemovedModels(unitModels);
                removedModels.ForEach(removedModels =>
                {
                    WorldCharacter worldCharacterToRemove = this.worldCharacters.Find(character => { return character.unitModel.ID == removedModels.ID; });
                    this.worldCharacters.Remove(worldCharacterToRemove);
                    worldCharacterToRemove.Destroy();
                });
            }));
            this.unitService.AddUnit(new UnitModel(.75f, new Vector3(1.729f, 0.966f, 0)));
        }

        // Update is called once per frame
        void Update()
        {

        }

        private WorldCharacter createUnit(UnitModel newUnit)
        {
            WorldCharacter newChar = this.characterFactory.Create(newUnit);
            newChar.transform.position = newUnit.position;
            return newChar;
        }
    }
}