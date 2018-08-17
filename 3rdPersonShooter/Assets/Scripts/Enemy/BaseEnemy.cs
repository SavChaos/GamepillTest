using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemy : BaseEntity
{
    public AIManager aiManager;

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

        //randomize look direction
        Vector3 eulerAngles = transform.rotation.eulerAngles;
        transform.rotation = Quaternion.Euler(eulerAngles.x, Random.Range(0, 360), eulerAngles.z);
    }

}
