using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEntity : BaseObject, ICollidable
{
    public float CurrentHealth;
    public Animator _animator;

    public enum AnimationState
    {
        Idle,
        Walk,
        Attack,
        Death
    }

    public AnimationState currentAnimState = AnimationState.Idle;

    // Use this for initialization
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update () {
		
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

    void Respawn()
    {
        ResetToDefault();
    }

    void ResetToDefault()
    {

    }

    void ChangeState(AnimationState animState)
    {
        currentAnimState = animState;
    }

    void OnDeath()
    {

    }

}
