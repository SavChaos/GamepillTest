using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

//Works with AI Manager to implement Attack Detection and Behavior for an enemy
public class AIAttackModule : MonoBehaviour
{
    public AIManager aiManager;
    public BoxCollider _collider;
    
    public const float ENEMY_DMG = 5f;

    public delegate void PlayerAttacked(float damage);
    public static PlayerAttacked OnPlayerAttacked;

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

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            aiManager.SwitchAIState(AIManager.AIState.Attacking);
            aiManager.detectionModule._collider.enabled = false;
        }
    }

    void OnTriggerStay(Collider collider)
    {
       
    }

    void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            aiManager.enemy._animator.SetBool("IsAttacking", false);
            aiManager.SwitchAIState(AIManager.AIState.Seeking);
            aiManager.detectionModule._collider.enabled = true;
        }
    }

   

}

