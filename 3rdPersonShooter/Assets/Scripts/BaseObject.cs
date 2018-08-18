﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseObject : MonoBehaviour, IPoolable
{
    public Rigidbody _rigidBody;
    public Collider _collider;
    public bool IsDead = false;
    public bool IsReserved = false; //special flag to alert that this object has been reserved by a caller for respawning
  
    // Use this for initialization
    protected virtual void Start ()
    {
		
	}
    
    protected virtual void OnDestroy()
    {

    }

    public virtual void Kill()
    {
        IsDead = true;
        IsReserved = false;
        gameObject.SetActive(false);
    }

    public virtual void Refresh(Vector3 startPos)
    {
        IsDead = false;
        gameObject.SetActive(true);
        transform.position = startPos;
        _collider.enabled = true;
    }

}

public interface ICollidable
{
    void OnCollisionEnter(Collision collision);
    void OnCollisionStay(Collision collision);
    void OnCollisionExit(Collision collision);
}

public interface ITriggerable
{
    void OnTriggerEnter(Collider collider);
    void OnTriggerStay(Collider collider);
    void OnTriggerExit(Collider collider);
}

public interface IPoolable
{
    void Refresh(Vector3 startPos);
    void Kill();
}
