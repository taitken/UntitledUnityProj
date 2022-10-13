using Item.Models;
using Unit.Models;
using UnityEngine;
using UtilityClasses;

namespace Characters
{
    public class UnitStateMoving : BaseUnitState
    {
        public UnitStateMoving() : base(eUnitState.Moving)
        {

        }

        public override void Update(WorldCharacter worldChar)
        {

        }

        public override void FixedUpdate(WorldCharacter worldChar)
        {
            if (worldChar.unitModel.currentPath != null && worldChar.unitModel.currentPath.Count > 0)
            {
                MovementHelper.MoveRigidBody2D(worldChar.GetComponent<Rigidbody2D>(), new Vector2(1, 1), worldChar.unitModel.moveSpeed, worldChar.unitModel.currentPath, worldChar.environmentService);
                this.UpdatePositions(worldChar);
                worldChar.pathingLine = MovementHelper.GetCharacterPathLine(worldChar.pathingLine, worldChar.pathLineFactory, worldChar.transform.position, worldChar.unitModel.currentPath, worldChar.environmentService);
            }
            else
            {
                worldChar.unitModel.unitState = eUnitState.Idle;
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