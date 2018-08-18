using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WanderTarget : MonoBehaviour, ITriggerable
{
    public AIManager aiManager;
    public SphereCollider sphereCollider;
    public MeshRenderer meshRenderer;

    void Awake()
    {
        if(!GameConfig.DEBUG_MODE)
        {
            meshRenderer.enabled = false;
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        Vector3 wanderTargetPos = transform.position;
    }

 
  
    public void OnTriggerExit(Collider other)
    {
    }

    public void OnTriggerStay(Collider other)
    {
    }
}
