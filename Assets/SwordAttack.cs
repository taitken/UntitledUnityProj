using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UtilityClasses;

public class SwordAttack : MonoBehaviour
{
    public Collider2D col;
    Vector2 attackOffset;
    public ActionController actionController;
    Transform tf;

    void Start()
    {
        this.tf = GetComponent<Transform>();
        this.attackOffset = transform.localPosition;
        Subscription sub = this.actionController.testobs.subscribe(delegate(string val)
        {
            print(val);
        });
        sub.unsubscribe();
    }
    public void Attack(bool flipX)
    {
       this.col.enabled = true;
        transform.localPosition = new Vector2(flipX ? -this.attackOffset.x : this.attackOffset.x, this.attackOffset.y);
    }

    public void StopAttack()
    {
        this.col.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {
            SlimeController slime = other.GetComponent<SlimeController>();
            if (slime)
            {
                slime.TakeDamage(3);
            }
        }
    }
}
