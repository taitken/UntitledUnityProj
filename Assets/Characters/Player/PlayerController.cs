using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Characters;
using GameControllers.Models;
using Zenject;
using Unit.Models;

namespace Characters
{
    public class PlayerController : WorldCharacter
    {
        public override float collisionOffset { get; set; } = 0.05f;
        public SwordAttack swordAttack;
        public ActionController actionController;
        Vector2 movementInput;
        private bool canMove = true;

        // Update is called once per frame
        private new void FixedUpdate()
        {
            base.FixedUpdate();
        }

        public void SwordAttack()
        {
            this.lockMovement();
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
