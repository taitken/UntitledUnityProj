using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Characters;
using GameControllers.Models;

namespace Characters
{
    public class PlayerController : WorldCharacter
    {
        public override float collisionOffset { get; set; } = 0.05f;
        public SwordAttack swordAttack;
        public ActionController actionController;
        Vector2 movementInput;
        private bool canMove = true;
        // Start is called before the first frame update
        void Start()
        {
        }
        // Update is called once per frame
        private new void FixedUpdate()
        {
            base.FixedUpdate();
            if (this.canMove)
            {
                this.moveUnit(this.movementInput);
            }
        }
        void OnMove(InputValue moveVal)
        {

            this.movementInput = moveVal.Get<Vector2>();
            this.setSpriteDirection(moveVal);
        }
        public void activateAttack()
        {
            this.animator.SetTrigger("attack");
        }

        private void setSpriteDirection(InputValue moveVal)
        {
            if (moveVal.Get<Vector2>().x != 0)
            {
                this.sr.flipX = moveVal.Get<Vector2>().x < 0;
            }
        }

        public void SwordAttack()
        {
            this.lockMovement();
            this.swordAttack.Attack(this.sr.flipX);
        }

        public void EndSwordAttack()
        {
            this.unlockMovement();
            this.swordAttack.StopAttack();
        }

        public void lockMovement()
        {
            this.canMove = false;
        }

        public void unlockMovement()
        {
            this.canMove = true;
        }
    }
}
