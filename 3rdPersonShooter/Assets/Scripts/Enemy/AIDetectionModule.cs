using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIDetectionModule : MonoBehaviour, ITriggerable
{
    public AIManager aiManager;
    public SphereCollider _collider;

    // Use this for initialization
    void Start()
    {

    }

    public void OnDisable()
    {
        _collider.enabled = false;
    }

    public void OnEnable()
    {
        _collider.enabled = true;
    }

    public void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            aiManager.PlayerDetected();
        }
    }

    public void OnTriggerStay(Collider collider)
    {
    }

    public void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            aiManager.PlayerUnDetected();
        }
    }

}



  

