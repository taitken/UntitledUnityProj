using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeController : MonoBehaviour
{
    Animator animator;
    public float HitPoints {
       get{
            return _hitPoints;
        }
        set{
            _hitPoints = value;
            if(_hitPoints <= 0)
            {
                this.Defeated();
            }
        }
    }

    private float _hitPoints = 2;

    // Start is called before the first frame update
    void Start()
    {
        this.animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage (float damage)
    {
        this.HitPoints -= damage;
    }

    public void Defeated(){
        this.animator.SetTrigger("onDeath");
    }

    public void RemoveUnit(){
        Destroy(gameObject);
    }
}
