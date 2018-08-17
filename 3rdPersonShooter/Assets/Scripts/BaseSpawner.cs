using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseSpawner : MonoBehaviour
{
    public float radius = 10;
    public float zOffset = 0;
    public float xOffset = 0;

    //Spawns an enemy in a random position within the Spawn Area
    public void SpawnEnemy(BaseEnemy enemy)
    {
        Debug.Log("Spawning Enemy");

        //find a random position within the spawn circle
        Vector2 randomPointSpawnCircle= (Random.insideUnitCircle.normalized * Random.Range(0, radius)) + new Vector2(transform.position.x, transform.position.z);

        enemy.Refresh(new Vector3(randomPointSpawnCircle.x, 0, randomPointSpawnCircle.y));
    }

    //Spawns a pickup in a random position within the Spawn Area
    public void SpawnPickup()
    {

    }

}
