using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIManager : MonoBehaviour
{
    public Rigidbody rigidBody;

    public float TotalHealth = 100f;
    public float CurrentHealth = 100f;

    //public Image healthBarFill;

    const float WANDER_SPEED = 2.5f;
    const float SEEK_SPEED = 5;

    public Animator animator;

    public GameObject player;
    public NavMeshAgent agent;

    public GameObject model;

    public GameObject WanderTarget;
    public GameObject WanderCenter;
    float wanderAngleTo;
    float turnSpeed = 30;

    public EnemyDetectionBehavior detectionScript;
    public EnemyAttackBehavior attackScript;

    Coroutine JitterCoroutine;
    bool createJitter = false;


    public enum AIState
    {
        Idle,
        Wandering,
        Seeking,
        Attacking,
        Dying
    }

    public AIState currentAIState;

    public void SwitchAIState(AIState aistate)
    {
        currentAIState = aistate;
    }
}
