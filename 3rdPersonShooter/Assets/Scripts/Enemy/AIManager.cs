using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIManager : MonoBehaviour
{   

    public BaseEnemy enemy;
    
    //public Image healthBarFill;

    const float WANDER_SPEED = 1f;
    const float SEEK_SPEED = 3.5f;
    const float RETURN_SPEED = 1.5f;
    const float JITTER_CHANGE_INTERVAL = 5f;
    
    public NavMeshAgent agent;

    public GameObject WanderTarget;
        
    public float wanderAngleTo;
    float turnSpeed = 30;
    float turnSpeedMult = 1;    //multiplier can be used to quickly ramp up the turn speed

    public AIDetectionModule detectionModule;
    public AIAttackModule attackModule;

    Coroutine JitterCoroutine;
    bool createJitter = false;
    bool createImmediateJitter = false;

    public BaseSpawner currentSpawner; 

    public float MinWaitForStateChange = 5;
    public float MaxWaitForStateChange = 15;

    public enum AIState
    {
        Idle,
        Wandering,
        Seeking,
        Attacking,
        Returning,
        Dying
    }

    public AIState currentAIState;

    public enum TurnState
    {
        Clockwise,
        CounterClockWise,
        NotTurning
    }

    public TurnState currentTurnState = TurnState.NotTurning;

    /*private void Awake()
    {
        Refresh(null);
    }*/

    //refresh AI behavior
    public void Refresh(BaseSpawner spawner)
    {
        currentSpawner = spawner;

        detectionModule.enabled = true;
        attackModule.enabled = true;

        //randomely pick between Idle or Wander when refreshing this enemy
        if (Random.value<0.5f)
        {
            SwitchAIState(AIState.Idle); //SwitchAIState(AIState.Idle);
        }
        else
        {
            SwitchAIState(AIState.Wandering);
        }

        float waitTime = Random.Range(MinWaitForStateChange, MaxWaitForStateChange);
        StartCoroutine(WaitChangeAIState(waitTime));
    }

    //wait a designated amount of time and switch between Idle and Wander states
    IEnumerator WaitChangeAIState(float waittime)
    {
        yield return new WaitForSeconds(waittime);

        if(currentAIState == AIState.Idle)
        {
            SwitchAIState(AIState.Wandering);
        }
        else if(currentAIState == AIState.Wandering)
        {
            SwitchAIState(AIState.Idle);
        }

        float waitTime = Random.Range(MinWaitForStateChange, MaxWaitForStateChange);
        StartCoroutine(WaitChangeAIState(waitTime));
    }

    public void SwitchAIState(AIState aistate)
    {       

        currentAIState = aistate;

        switch (currentAIState)
        {
            case AIState.Idle:
                SetIdle();
                break;

            case AIState.Wandering:
                SetWander();
                break;

            case AIState.Seeking:
                SetSeek();
                break;

            case AIState.Attacking:
                SetAttack();
                break;

            case AIState.Returning:
                SetReturn();
                break;

            case AIState.Dying:
                SetDeath();              
                break;
        }
    }

    IEnumerator RemoveEnemy()
    {
        yield return new WaitForSeconds(4);
        Destroy(gameObject);
    }

    void SetIdle()
    {
        agent.enabled = false;
        enemy._rigidBody.velocity = Vector3.zero;

        if (JitterCoroutine != null)
        {
            StopCoroutine(JitterCoroutine);
            JitterCoroutine = null;
        }

        enemy._animator.SetBool("IsMoving", false);
        enemy._rigidBody.mass = 100; //make enemy unmovable
        enemy._rigidBody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationY |
                                        RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ;
    }

    void SetWander()
    {
        agent.enabled = false;
        ImmediateJitter();
        enemy._animator.SetBool("IsMoving", true);
        enemy._rigidBody.mass = 10;
        enemy._rigidBody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationY |
                                       RigidbodyConstraints.FreezePositionY;
    }

    void SetSeek()
    {
        agent.enabled = true;
        agent.speed = SEEK_SPEED;
        enemy._animator.SetBool("IsSeeking", true);
        enemy._rigidBody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationY |
                                       RigidbodyConstraints.FreezePositionY;
    }

    void SetReturn()
    {
        agent.enabled = true;
        agent.speed = RETURN_SPEED;
        enemy._animator.SetBool("IsMoving", true);
        enemy._rigidBody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationY |
                                       RigidbodyConstraints.FreezePositionY;
    }

    void SetAttack()
    {
        agent.enabled = true;
        enemy._animator.SetBool("IsAttacking", true);
        
        enemy._rigidBody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationY |
                                       RigidbodyConstraints.FreezePositionY;
    }

    void SetDeath()
    {
        enemy._rigidBody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationY |
                                    RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ;

        agent.enabled = false;
        detectionModule.enabled = false;
        attackModule.enabled = false;
        currentSpawner.currentEnemySpawnCount--; //remove this Enemy from its spawn Area Count
    }
   
    void Return()
    {
        if (agent.enabled)
        {
            agent.SetDestination(currentSpawner.spawnBounds.pos);
        }
                
        ResetModelRotation();

        Vector2 enemyTopDownPos = new Vector2(transform.position.x, transform.position.z);
        if(Utils.WithinBounds(enemyTopDownPos, currentSpawner.spawnBounds))
        {
            SwitchAIState(AIState.Wandering);
        }
    }
    

    void Seek()
    {
        if (Main.GetInstance().currentPlayer.IsDead)
        {
            SwitchAIState(AIState.Wandering);
            return;
        }

        if (agent.enabled)
        {
            agent.SetDestination(Main.GetInstance().currentPlayer.transform.position);
        }
        
        ResetModelRotation();
    }

    void Attacking()
    {
        if (Main.GetInstance().currentPlayer.IsDead)
        {
            SwitchAIState(AIState.Wandering);
        }
    }

    //this forces an immediate randomized jitter
    void ImmediateJitter()
    {
        wanderAngleTo += Random.Range(0, 360);
        //Debug.LogError("WANDER TO [" + wanderAngleTo + "]");
        createJitter = false;
    }

    //this forces sets a jitter as an input
    void ImmediateJitter(float angle)
    {
        wanderAngleTo += angle;
        //Debug.LogError("WANDER TO [" + wanderAngleTo + "]");
        createJitter = false;
    }

    //this forces sets a jitter as an input
    void SetWanderAngle(float angle)
    {
        wanderAngleTo = angle;
        //Debug.LogError("Hard Set WANDER TO [" + wanderAngleTo + "]");
        createJitter = false;
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentAIState)
        {
            case AIState.Idle:
                Idling();
                break;

            case AIState.Wandering:
                Wander();
                break;

            case AIState.Seeking:
                Seek();
                break;

            case AIState.Attacking:
                Attacking();
                break;

            case AIState.Returning:
                Return();
                break;

            case AIState.Dying:
                break;
        }
    }

    public void PlayerDetected()
    {
        SwitchAIState(AIState.Seeking);
    }

    public void PlayerUnDetected()
    {
        //once you have enemy attention, they will hound you relentlessly
        if (enemy._aggro)
            return;

        SwitchAIState(AIState.Wandering);
        enemy._animator.SetBool("IsSeeking", false);
    }

    void Idling()
    {
        if(enemy._aggro)
        {
            SwitchAIState(AIState.Seeking);
            return;
        }
    }

    void Wander()
    {
        //first check if this AI is within the Wander bounds
        //if out of bounds, Switch AI State to RETURN
        if (!CheckAIIsWithinWanderBounds())
        {
            return;
        }

        //if enemy is Aggroed, then they can no longer wonder, they will chase you endlessly
        if(enemy._aggro)
        {
            SwitchAIState(AIState.Seeking);
            return;
        }

        if (!createJitter && !createImmediateJitter)
        {
            JitterCoroutine = StartCoroutine(Jitter(JITTER_CHANGE_INTERVAL));    //start a new jitter every time interval
            createJitter = true;
        }

        Vector3 dir = WanderTarget.transform.position - enemy.transform.position; //WanderCenter.transform.position;

        float angle = Mathf.Atan2(dir.z, dir.x) * Mathf.Rad2Deg;

        float deltaAngle = Mathf.DeltaAngle(wanderAngleTo, angle);

        if (Mathf.Abs(deltaAngle) < turnSpeed * turnSpeedMult * Time.deltaTime)
        {
            //Debug.Log("ANGLE REACHED!");
            currentTurnState = TurnState.NotTurning;
            createImmediateJitter = false;
        }
        else
        {
            if (deltaAngle < 0)
            {
                WanderTarget.transform.RotateAround(enemy.transform.position, //WanderCenter.transform.position,
                    Vector3.up, -turnSpeed * turnSpeedMult * Time.deltaTime);
                currentTurnState = TurnState.CounterClockWise;
            }
            else
            {
                WanderTarget.transform.RotateAround(enemy.transform.position, //WanderCenter.transform.position,
                    Vector3.up, turnSpeed * turnSpeedMult * Time.deltaTime);
                currentTurnState = TurnState.Clockwise;
            }
        }

        Vector3 dirToTarget = WanderTarget.transform.position - enemy.model.transform.position;

        Vector3 dirWithoutY = new Vector3(dirToTarget.x, 0, dirToTarget.z);

        dirWithoutY.Normalize();

        Vector3 velocity = new Vector3(dirWithoutY.x * WANDER_SPEED,
                                            enemy._rigidBody.velocity.y,
                                                dirWithoutY.z * WANDER_SPEED);

      
        enemy._rigidBody.velocity = (createImmediateJitter) ? Vector3.zero : velocity;
        

        //FACE THE TARGET       
        Quaternion rotationToFace = Quaternion.LookRotation(dirWithoutY, Vector3.up);
        enemy.model.transform.rotation = Quaternion.Slerp(enemy.model.transform.rotation, rotationToFace, 0.5f);

        //check if this AI's wander target is within bounds
        //modify jitter and keep AI within Bounds
        CheckIfTargetIsWithinWanderBounds();

    }
       
    void CheckIfTargetIsWithinWanderBounds()
    {
        Vector2 wanderTarget2DPos = new Vector2(WanderTarget.transform.position.x, WanderTarget.transform.position.z);
        Vector2 enemy2DPos = new Vector2(transform.position.x, transform.position.z);
        Vector2 dirTo = wanderTarget2DPos - enemy2DPos;
        float wanderTargetAngle = Mathf.Atan2(dirTo.y, dirTo.x);

        //if we have wandered out of spawn bounds
        if (!Utils.WithinBounds(wanderTarget2DPos, currentSpawner.spawnBounds))
        {
            if (!createImmediateJitter)
            {
                //Debug.LogError("out of bounds");
                int check = CheckDirectionToTurn();
                float newAngle = (wanderTargetAngle * Mathf.Rad2Deg) + (check * 90);
                //Debug.LogError("[OOB] Adding Emergency Jitter of " + check * 90  +  "   new angle: " + newAngle);

                //create immediate jiter in clockwise or counter clockwise direction from the current wander target angle
                SetWanderAngle(newAngle);
                createImmediateJitter = true;

                if (JitterCoroutine != null)
                {
                    StopCoroutine(JitterCoroutine);
                    JitterCoroutine = null;
                }

                turnSpeedMult = 5;
            }
        }
        else
        {
            createImmediateJitter = false;
            turnSpeedMult = 1;
        }
    }

    bool CheckAIIsWithinWanderBounds()
    {
        Circle2D exagerratedSpawnBounds = currentSpawner.spawnBounds; 
        //we take an area that is 25% larger than the actual spawn bounds, giving some flexibility
        exagerratedSpawnBounds.radius *= 1.25f;
        
        Vector2 enemyTopDownPos = new Vector2(transform.position.x, transform.position.z);
        if (!Utils.WithinBounds(enemyTopDownPos, exagerratedSpawnBounds))
        {
            SwitchAIState(AIState.Returning);
            return false;
        }

        return true;
    }

    //when meetings another enemy, decide which is the best direction to turn away to avoid the enemy
    public void TurnAwayFromEnemy(Transform otherEnemy)
    {
        //only turn away if wondering
        if (currentAIState != AIState.Wandering)
            return;
        
        //Debug.LogError("Turning away from enemy");

        if (!createImmediateJitter)
        {
            
            float angleOfWanderTarget = GetAngleOfObject(WanderTarget.transform);
            float angleOfOtherEnemy = GetAngleOfObject(otherEnemy);

            if(angleOfWanderTarget<angleOfOtherEnemy)
            {
               //Debug.LogError("Turning ClockWise to avoid enemy [" + name +"]");
                //create immediate jiter in clockwise or counter clockwise direction from the current wander target angle
                SetWanderAngle((angleOfWanderTarget*Mathf.Rad2Deg) - 45);
            }
            else
            {
                //Debug.LogError("Turning Counter clockwise to avoid enemy [" + name +"]");
                //create immediate jiter in clockwise or counter clockwise direction from the current wander target angle
                SetWanderAngle((angleOfWanderTarget * Mathf.Rad2Deg) + 45);
            }

            if (JitterCoroutine != null)
            {
                StopCoroutine(JitterCoroutine);
                JitterCoroutine = null;
            }
            turnSpeedMult = 5;
            createImmediateJitter = true;
        }
    }

    //get the angle of an object with respect this object
    float GetAngleOfObject(Transform _obj)
    {
        Vector2 otherObjectPos2D = new Vector2(_obj.transform.position.x, _obj.transform.position.z);
        Vector2 thisObjPos2D = new Vector2(transform.position.x, transform.position.z);
        Vector2 dirTo = otherObjectPos2D - thisObjPos2D;
        return Mathf.Atan2(dirTo.y, dirTo.x);
    }

    //this checks if the wander target will be in within the wander bounds if turning 90 degrees to the right or left
    //return -1 if its safe to turn clockwise and 1 if its safe to turn counter clockwise
    int CheckDirectionToTurn()
    {
        Vector2 wanderTarget2DPos = new Vector2(WanderTarget.transform.position.x, WanderTarget.transform.position.z);
        Vector2 enemyTopDownPos = new Vector2(transform.position.x, transform.position.z);
        Vector2 dirTo = wanderTarget2DPos - enemyTopDownPos;
        float wanderTargetAngle = Mathf.Atan2(dirTo.y, dirTo.x);

        //Debug.Log(" Wander Target Angle at time Of Emergency [" + wanderTargetAngle*Mathf.Rad2Deg + "]");

        //Check ClockWise 90 degree shift
        float dist = Vector2.Distance(wanderTarget2DPos, enemyTopDownPos);
        float newAngle = wanderTargetAngle - Mathf.PI / 2f;
        float vx = Mathf.Cos(newAngle) * dist;
        float vy = Mathf.Sin(newAngle) * dist;
        Vector2 turnedPos = enemyTopDownPos + new Vector2(vx, vy);

        if(Utils.WithinBounds(turnedPos, currentSpawner.spawnBounds))
        {
            //Debug.LogError("safe to turn clockwise");
            return -1;
        }

        newAngle = wanderTargetAngle + Mathf.PI / 2f;
        vx = Mathf.Cos(newAngle) * dist;
        vy = Mathf.Sin(newAngle) * dist;
        turnedPos = enemyTopDownPos + new Vector2(vx, vy);

        if (Utils.WithinBounds(turnedPos, currentSpawner.spawnBounds))
        {
            //Debug.LogError("safe to turn COUNTER clockwise");
            return 1;
        }

        return 1;
    }

    //sets the model to align with the player forward vector
    void ResetModelRotation()
    {
        Quaternion rot = enemy.model.transform.localRotation;

        enemy.model.transform.localRotation = Quaternion.Slerp(rot, Quaternion.identity, 0.1f);
    }

    public void AttackHit()
    {
        AIAttackModule.OnPlayerAttacked(AIAttackModule.ENEMY_DMG);
    }


    IEnumerator Jitter(float time)
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(time);

        yield return waitForSeconds;

        wanderAngleTo -= Random.Range(-10, 11) * 10;

        //Debug.LogError("Adding natural Jitter new Wander Angle [" + wanderAngleTo + "]");
        createJitter = false;
    }
}
