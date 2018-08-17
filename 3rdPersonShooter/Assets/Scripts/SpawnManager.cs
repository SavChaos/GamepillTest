using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public List<BaseEnemy> enemyPoolList;
    public List<BaseSpawner> spawners;
    public GameObject BruteEnemy_Prefab;
    public GameObject ZombieEnemy_Prefab;
    public GameObject MutantEnemy_Prefab;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.S))
        {
            BaseEnemy.EnemyType enemyTypeToSpawn = BaseEnemy.EnemyType.Brute;

            //return enemy from pool
            foreach (BaseEnemy enemy in enemyPoolList)
            {
                if (enemy.enemyType == enemyTypeToSpawn && enemy.IsDead)
                {
                    spawners[0].SpawnEnemy(enemy);
                    return;
                }
            }

            //if there is no object to use from pool, we instantiate a new one and store in the pool
            BaseEnemy _enemy = CreateEnemy(enemyTypeToSpawn);
            enemyPoolList.Add(_enemy);
            spawners[0].SpawnEnemy(_enemy);
        }
    }

    BaseEnemy CreateEnemy(BaseEnemy.EnemyType enemyType)
    {
        BaseEnemy _enemy = null;

        switch (enemyType)
        {
            case BaseEnemy.EnemyType.Brute:
                _enemy = Instantiate(BruteEnemy_Prefab).GetComponent<BaseEnemy>();
                break;

            case BaseEnemy.EnemyType.Zombie:
                _enemy = Instantiate(ZombieEnemy_Prefab).GetComponent<BaseEnemy>();
                break;

            case BaseEnemy.EnemyType.Mutant:
                _enemy = Instantiate(MutantEnemy_Prefab).GetComponent<BaseEnemy>();
                break;
        }

        return _enemy;
    }


}
