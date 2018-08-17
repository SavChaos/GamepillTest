using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammo : BaseObject
{
    Vector3 prevDirToContact;
    Vector3 contactPoint; //every bullet knows the end point of its trajectory
    Vector3 shootDir;

    // Use this for initialization
    protected override void Start ()
    {
		
	}

    public void Fire(Ray shootRay, float force)
    {

        Debug.Log("raycasting");
        RaycastHit hit;

        //create a raycast in the direction
        //we are always guaranteed to make contact with a surface in the level
        if (Physics.Raycast(shootRay, out hit))
        {
            if (hit.collider != null)
            {
                Debug.LogError("RAY CAST HIT [" + hit.collider.gameObject.name + "]");
                contactPoint = hit.point;
                Vector3 bulletVelocity = shootRay.direction.normalized;
                prevDirToContact = (transform.position - contactPoint).normalized;
                _rigidBody.velocity = bulletVelocity * force;
            }
        }
        
    }

    void FixedUpdate()
    {
        Vector3 dirToContact = (transform.position - contactPoint).normalized;
        Vector3 shootDir = _rigidBody.velocity.normalized;

        Vector3 diff = dirToContact - shootDir;
        
       // Debug.Log("ToContact x " + dirToContact.x + " y " + dirToContact.y + " z " + dirToContact.z + "   shootDir x " + shootDir.x + " y " + shootDir.y + " z " + shootDir.z + "   diff x " + diff.x + " y " + diff.y + " z " + diff.z);

        //if any of the direction axis flipped from positive to negative or vice versa, we have moved past our contact point
        if(dirToContact.x > 0 && prevDirToContact.x < 0 || dirToContact.x < 0 && prevDirToContact.x > 0
            || dirToContact.z > 0 && prevDirToContact.z < 0 || dirToContact.z < 0 && prevDirToContact.z > 0)
        {
            Kill();
        }

        prevDirToContact = dirToContact;
    }

   
    /*public void OnTriggerEnter(Collider other)
    { 
        if (other.gameObject.layer != LayerMask.NameToLayer("Player") && other.gameObject.layer != LayerMask.NameToLayer("Ammo"))
        {
            Kill();
            Debug.Log("TRIGGER ENTER with [" + other.gameObject.name + "]");
        }
    }

    public void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer("Player"))
        {
            Kill();
            Debug.Log("TRIGGER STAY with [" + other.gameObject.name + "]");
        }
    }

    public void OnTriggerExit(Collider other)
    {

    }*/
}
