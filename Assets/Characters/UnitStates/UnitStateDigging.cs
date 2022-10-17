using System.Collections.Generic;
using Environment.Models;
using GameControllers.Models;
using ObjectComponents;
using Unit.Models;
using UnityEngine;
using UtilityClasses;

namespace Characters
{
    public class UnitStateDigging : BaseUnitState
    {
        private float digInterval = 1f;
        private float timeSinceLastDig = 0;
        private Vector3 originalPosition;
        private Vector3 direction;
        private bool currentlyDigging = false;
        private bool dealtDamageThisDig = false;
        private IList<Vector3> digAnimPath = new List<Vector3>();
        public UnitStateDigging() : base(eUnitState.Digging)
        {
            this.timeSinceLastDig = this.digInterval;
        }
        
        public override void Initialise(WorldCharacter worldChar)
        {

        }

        // Convoluted -- should be a better way than bool flags.
        public override void Update(WorldCharacter worldChar)
        {
            if (worldChar.unitModel.currentOrder is DigOrderModel)
            {
                this.timeSinceLastDig += GameTime.deltaTime;
                if (this.timeSinceLastDig >= this.digInterval)
                {
                    this.timeSinceLastDig = this.timeSinceLastDig - this.digInterval;
                    this.StartDig(worldChar);
                }
                if (this.currentlyDigging && !this.dealtDamageThisDig)
                {
                    if (this.digAnimPath.Count < 2)
                    {
                        MineableObjectModel mineTarget = (worldChar.unitModel.currentOrder as DigOrderModel).targetToMine;
                        mineTarget.GetObjectComponent<ObjectHitPointsComponent>().TakeDamage(25f);
                        this.dealtDamageThisDig = true;
                    }
                }
            }
            else
            {
                worldChar.unitModel.unitState = eUnitState.Idle;
            }

        }

        public override void FixedUpdate(WorldCharacter worldChar)
        {
            //this.HandleDigAnimation(worldChar);
        }

        private void StartDig(WorldCharacter worldChar)
        {
            this.currentlyDigging = true;
            this.dealtDamageThisDig = false;
            if (this.direction == default(Vector3)) this.FaceBlock(worldChar);
            if (this.originalPosition == default(Vector3)) this.originalPosition = new Vector3(worldChar.transform.position.x, worldChar.transform.position.y);
            if (this.digAnimPath.Count == 0)
            {
                this.digAnimPath.Add(new Vector3(worldChar.transform.position.x - (.05f * this.direction.x), worldChar.transform.position.y - (.05f * this.direction.y)));
                this.digAnimPath.Add(new Vector3(worldChar.transform.position.x + (.05f * this.direction.x), worldChar.transform.position.y + (.05f * this.direction.y)));
                this.digAnimPath.Add(this.originalPosition);
            }
            MovementSingleton.GetMovementHelper().MoveObject(worldChar.transform, new Vector2(1, 1), .75f, this.digAnimPath, false, false).onMovementFinished.OnEmit(()=>this.currentlyDigging = false);
        }

        private void FaceBlock(WorldCharacter worldChar)
        {
            MineableObjectModel mineTarget = (worldChar.unitModel.currentOrder as DigOrderModel).targetToMine;
            this.direction = MovementSingleton.GetMovementHelper().GetDirection(worldChar.transform.position, worldChar.environmentService.CellToLocal(mineTarget.position));
            MovementSingleton.GetMovementHelper().UpdateSpriteDirection(worldChar.gameObject, this.direction);
        }
    }
}