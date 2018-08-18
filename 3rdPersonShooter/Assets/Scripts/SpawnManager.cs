using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public float BASE_SPAWN_INTERVAL = 10;
    public List<BaseEnemy> enemyPoolList;
    public List<BaseSpawner> spawners;
    public GameObject BruteEnemy_Prefab;

    private void Start()
    { 
        StartCoroutine(SpawnEnemiesEachInterval());
    }

    IEnumerator SpawnEnemiesEachInterval()
    {
        while (!Main.GAME_OVER)
        {
            SpawnEnemies();

            yield return new WaitForSeconds(BASE_SPAWN_INTERVAL);
        }
    }

    void SpawnEnemies()
    {
        BaseEnemy.EnemyType enemyTypeToSpawn = BaseEnemy.EnemyType.Brute;

        foreach (BaseSpawner spawner in spawners)
        {
            if (spawner.FullCapacity)   //skip spawners that are at full capacity
                continue;

            Debug.LogError("spawner spawn");

            bool recyled = false;

            //return enemy from pool
            foreach (BaseEnemy enemy in enemyPoolList)
            {
                if (enemy.enemyType == enemyTypeToSpawn && enemy.IsDead && !enemy.IsReserved)
                {
                    spawner.SpawnEnemy(enemy);
                    recyled = true;

                    break;
                }
            }

            if (!recyled)
            {
                //if there is no object to use from pool, we instantiate a new one and store in the pool
                BaseEnemy _enemy = CreateEnemy(enemyTypeToSpawn);
                enemyPoolList.Add(_enemy);
                spawner.SpawnEnemy(_enemy);
            }
        }
    }

    void Update()
    {        

        if (Input.GetKeyDown(KeyCode.P))
        {
            SpawnEnemies();
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
