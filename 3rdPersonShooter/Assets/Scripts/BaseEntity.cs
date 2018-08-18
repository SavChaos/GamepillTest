using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEntity : BaseObject, ICollidable
{
    public float TotalHealth;
    public float CurrentHealth;

    public GameObject model;
    public Animator _animator;


    // Use this for initialization
    protected override void Start()
    {
        base.Start();
    }


    public virtual void OnCollisionEnter(Collision collision)
    {
        //Debug.Log("[" + name + "] on collision enter with [" + collision.gameObject.name + "]");
    }

    public virtual void OnCollisionExit(Collision collision)
    {
        //Debug.Log("[" + name + "] on collision exit with [" + collision.gameObject.name + "]");
    }

    public virtual void OnCollisionStay(Collision collision)
    {
        //Debug.Log("[" + name + "] on collision stay with [" + collision.gameObject.name + "]");
    }

    protected void Respawn()
    {
        ResetToDefault();
    }

    protected void ResetToDefault()
    {

    }


    public void ChangeHealth(float change)
    {
        CurrentHealth += change;
        CurrentHealth = Mathf.Clamp(CurrentHealth, 0, TotalHealth);
        if (CurrentHealth <= 0)
        {
            OnDeath();
        }
    }

  

    protected virtual void OnDeath()
    {
        _collider.enabled = false;
        _animator.SetBool("IsDead", true);
    }

}
