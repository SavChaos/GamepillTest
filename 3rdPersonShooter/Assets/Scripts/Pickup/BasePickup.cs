using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePickup : BaseObject, ITriggerable
{
    public float rotationAngle;

    // Use this for initialization
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        //rotate pickup
        transform.Rotate(0, rotationAngle, 0);
    }

    public virtual void OnTriggerEnter(Collider collider)
    {
        Kill();
    }

    public virtual void OnTriggerExit(Collider collider)
    {
    }

    public virtual void OnTriggerStay(Collider collider)
    {
    }

    
}
