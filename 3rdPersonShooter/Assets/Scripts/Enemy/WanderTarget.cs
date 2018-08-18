using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WanderTarget : MonoBehaviour, ITriggerable
{
    public AIManager aiManager;
    public SphereCollider sphereCollider;

    public void OnTriggerEnter(Collider other)
    {
        //Debug.Log("wander");

        Vector3 wanderTargetPos = transform.position;

       /* if(aiManager.currentTurnState == AIManager.TurnState.Clockwise)
        {

        }
        else if (aiManager.currentTurnState == AIManager.TurnState.CounterClockWise)
        {

        }
        else
        {
            //do something else

        }*/

    }

 
  
    public void OnTriggerExit(Collider other)
    {
    }

    public void OnTriggerStay(Collider other)
    {
    }
}
