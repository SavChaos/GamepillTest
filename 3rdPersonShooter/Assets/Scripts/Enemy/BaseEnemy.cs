using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemy : BaseEntity
{
    public AIManager aiManager;
    public float CLEAN_UP_TIME = 5f;
    public EnemyType enemyType;
    public enum EnemyType
    {
        Brute,
        Zombie,
        Mutant
    }

    // Use this for initialization
    protected override void Start()
    {
        base.Start();
    }

    public override void Kill()
    {
        base.Kill();
    }

    public override void Refresh(Vector3 startPos)
    {
        base.Refresh(startPos);

        CurrentHealth = TotalHealth;    //reset health

        //randomize look direction
      //  Vector3 eulerAngles = transform.rotation.eulerAngles;
      //  transform.rotation = Quaternion.Euler(eulerAngles.x, Random.Range(0, 360), eulerAngles.z);
    }

    protected override void OnDeath()
    {
        aiManager.SwitchAIState(AIManager.AIState.Dying);
        base.OnDeath();
        StartCoroutine(WaitCleanUpEnemy());
    }

    IEnumerator WaitCleanUpEnemy()
    {
        yield return new WaitForSeconds(CLEAN_UP_TIME);

        Kill();
    }

    public override void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);

        if(collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            aiManager.TurnAwayFromEnemy(collision.gameObject.transform);
        }
    }  

}
