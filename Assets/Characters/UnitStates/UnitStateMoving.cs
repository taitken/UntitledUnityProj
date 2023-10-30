using GameControllers.Models;
using Item.Models;
using Unit.Models;
using UnityEngine;
using UtilityClasses;

namespace Characters
{
    public class UnitStateMoving : BaseUnitState
    {
        private ObjectMovement objectMovement;
        public UnitStateMoving() : base(eUnitState.Moving)
        {

        }

        public override void Initialise(WorldCharacter worldChar)
        {
            float moveSpeed = worldChar.unitModel.currentOrder is WanderOrderModel ? WanderOrderModel.WANDER_SPEED : worldChar.unitModel.moveSpeed;
            this.objectMovement = MovementSingleton.GetMovementHelper().MoveObject(worldChar.transform, new Vector2(1, 1), moveSpeed, worldChar.unitModel.currentPath);
            this.objectMovement.onMovementFinished.OnEmit(() =>
            {
                if (worldChar.unitModel.currentPath.Count == 0)
                    worldChar.unitModel.unitState = eUnitState.Idle;
            });
        }
        public override void Update(WorldCharacter worldChar)
        {

        }

        public override void FixedUpdate(WorldCharacter worldChar)
        {
            if (worldChar.unitModel.currentPath != null && worldChar.unitModel.currentPath.Count > 0)
            {
                this.UpdatePositions(worldChar);
            }
        }

        private void UpdatePositions(WorldCharacter worldChar)
        {
            worldChar.unitModel.localPosition = new Vector3(worldChar.gameObject.transform.position.x + worldChar.unitModel.spriteOffset, worldChar.gameObject.transform.position.y + worldChar.unitModel.spriteOffset);
            worldChar.unitModel.position = worldChar.environmentService.LocalToCell(worldChar.unitModel.localPosition);
            if (worldChar.unitModel.carriedItem != null && worldChar.unitModel.carriedItem.itemState == ItemObjectModel.eItemState.OnCharacter) worldChar.unitModel.carriedItem.position = worldChar.unitModel.position;
        }
    }
}