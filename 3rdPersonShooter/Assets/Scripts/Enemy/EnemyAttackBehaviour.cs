using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class EnemyAttackBehavior : MonoBehaviour
{
    public AIManager aiManager;
    public Animator animator;
    public PlayerManager playerScript;

    bool attackStarted = false;

    const float ENEMY_DMG = 5f;
    const float ENEMY_ATTACK_SPEED = 1.5f;

    public delegate void PlayerAttacked(float damage);
    public static PlayerAttacked OnPlayerAttacked;

    // Use this for initialization
    void Start()
    {

    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            animator.SetBool("IsAttacking", true);
            aiManager.SwitchAIState(AIManager.AIState.Attacking);
        }
    }

    void OnTriggerStay(Collider collider)
    {
        if (collider.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if (aiManager.currentAIState == AIManager.AIState.Attacking)
            {
                if (!attackStarted)
                {
                    StartCoroutine(AttackProcess());
                    attackStarted = true;
                }
            }
        }
    }

    void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            animator.SetBool("IsAttacking", false);
            aiManager.SwitchAIState(AIManager.AIState.Seeking);
        }
    }

    IEnumerator AttackProcess()
    {
        OnPlayerAttacked(ENEMY_DMG);

        Debug.Log("!!!DMG!!!");

        yield return new WaitForSeconds(ENEMY_ATTACK_SPEED);

        attackStarted = false;
    }

    // Update is called once per frame
    void Update()
    {

    }
}

