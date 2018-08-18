using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public float MAX_SPAWN_INTERVAL = 10;
    public float SPAWN_ENEMIES_INTERVAL = 10;
    public float SPAWN_PICKUPS_INTERVAL = 10;
    [Tooltip ("Sets the % chance for each spawner to spawn a pickup during the spawn phase (0 - 1 Range)")]
    [Range(0f, 1f)]
    public float pickupSpawnChance = 0.5f;
    public List<BaseEnemy> enemyPoolList;
    public List<BasePickup> pickupPoolList;
    public List<BaseSpawner> spawners;
    public GameObject BruteEnemy_Prefab;
    public GameObject HealthPickup_Prefab;
    public GameObject SpeedPickup_Prefab;

    private void Start()
    { 
        StartCoroutine(SpawnEnemiesEachInterval());
        StartCoroutine(SpawnPickupsEachInterval());
    }
       
    IEnumerator SpawnEnemiesEachInterval()
    {
        while (!Main.GAME_OVER)
        {
            SpawnEnemies();

            yield return new WaitForSeconds(SPAWN_ENEMIES_INTERVAL);
        }
    }

    IEnumerator SpawnPickupsEachInterval()
    {
        while (!Main.GAME_OVER)
        {
            SpawnPickups();

            yield return new WaitForSeconds(SPAWN_PICKUPS_INTERVAL);            
        }
    }

    void SpawnPickups()
    {
        foreach (BaseSpawner spawner in spawners)
        {
            if (!spawner.isActiveAndEnabled || spawner.FullEnemyCapacity)   //skip spawners that are at disabled or at full capacity
                continue;
            
            bool recyled = false;

            float spawnChance = Random.value;

            //THis spawner didn't meet the probability of spawning a pickup
            if (spawnChance > pickupSpawnChance)
                continue;

            //randomly pick between health and speed pickup
            int pickupType = Random.Range(0, 2);
            BasePickup.PickupType pickUpTypeToSpawn = (pickupType == 0) ? BasePickup.PickupType.Health : BasePickup.PickupType.Speed;

            //return enemy from pool
            foreach (BasePickup pickup in pickupPoolList)
            {
                if (pickup.pickupType == pickUpTypeToSpawn && pickup.IsDead && !pickup.IsReserved)
                {
                    spawner.SpawnPickup(pickup);
                    recyled = true;

                    break;
                }
            }

            if (!recyled)
            {
                //if there is no object to use from pool, we instantiate a new one and store in the pool
                BasePickup _pickup = CreatePickup(pickUpTypeToSpawn);
                pickupPoolList.Add(_pickup);
                spawner.SpawnPickup(_pickup);
            }
        }
    }

    void SpawnEnemies()
    {
        BaseEnemy.EnemyType enemyTypeToSpawn = BaseEnemy.EnemyType.Brute;

        foreach (BaseSpawner spawner in spawners)
        {
            if (!spawner.isActiveAndEnabled || spawner.FullEnemyCapacity)   //skip spawners that are at disabled or at full capacity
                continue;
            
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

    

    BaseEnemy CreateEnemy(BaseEnemy.EnemyType enemyType)
    {
        BaseEnemy _enemy = null;

        switch (enemyType)
        {
            case BaseEnemy.EnemyType.Brute:
                _enemy = Instantiate(BruteEnemy_Prefab).GetComponent<BaseEnemy>();
                break;
        }

        return _enemy;
    }

    BasePickup CreatePickup(BasePickup.PickupType pickupType)
    {
        BasePickup _pickup = null;

        switch (pickupType)
        {
            case BasePickup.PickupType.Health:
                _pickup = Instantiate(HealthPickup_Prefab).GetComponent<BasePickup>();
                break;

            case BasePickup.PickupType.Speed:
                _pickup = Instantiate(SpeedPickup_Prefab).GetComponent<BasePickup>();
                break;
        }

        return _pickup;
    }


}
