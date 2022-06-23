using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public SwordAttack swordAttack;
    public float moveSpeed = 1f;
    public float collisionOffset = 0.05f;
    public ContactFilter2D movementFilter;
    Vector2 movementInput;
    Rigidbody2D rb;
    Animator animator;
    SpriteRenderer sr;

    private bool canMove = true;

    List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (this.canMove)
        {
            this.moveUnit(this.movementInput);
        }
    }

    private void moveUnit(Vector2 movement)
    {
        if (this.movementInput != Vector2.zero)
        {
            bool horizontalCollison = this.collisionCheck(new Vector2(movement.x, 0));
            bool verticalCollison = this.collisionCheck(new Vector2(0, movement.y));
            rb.MovePosition(rb.position + new Vector2(horizontalCollison ? 0 : movement.x, verticalCollison ? 0 : movement.y) * moveSpeed * Time.fixedDeltaTime);
            if (!horizontalCollison || !verticalCollison)
            {
            }
        }
        this.animator.SetBool("isMoving", this.movementInput != Vector2.zero);
    }

    private bool collisionCheck(Vector2 movement)
    {
        int count = rb.Cast(
            movement,
            movementFilter,
            castCollisions,
            moveSpeed * Time.fixedDeltaTime + collisionOffset
        );
        return count != 0;
    }

    void OnMove(InputValue moveVal)
    {

        this.movementInput = moveVal.Get<Vector2>();
        this.setSpriteDirection(moveVal);
    }

    void OnFire()
    {
        //this.animator.SetTrigger("attack");
    }

    public void activateAttack(){
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
