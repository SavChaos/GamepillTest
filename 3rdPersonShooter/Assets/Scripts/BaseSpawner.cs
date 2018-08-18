using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseSpawner : MonoBehaviour
{
    public float Max_Interval_Delay = 5;    //each spawner can randomely add an extra custom delay
    public float radius = 10;
    public float zOffset = 0;
    public float xOffset = 0;
    public Circle2D spawnBounds;
    public int Spawn_Limit = 5; //defines how many can spawn into this spawn area
    public int currentSpawnCount;

    public bool FullCapacity
    {
        get
        {
            return (currentSpawnCount >= Spawn_Limit);
        }
    }

    private void Awake()
    {
        spawnBounds = new Circle2D(new Vector2(transform.position.x, transform.position.z), new Vector2(xOffset, zOffset), radius);
    }

    //Spawns an enemy in a random position within the Spawn Area
    public void SpawnEnemy(BaseEnemy enemy)
    {
        enemy.gameObject.SetActive(false);
        enemy.IsReserved = true;
        float rndDelay = Random.Range(0, Max_Interval_Delay);
        currentSpawnCount++;
        StartCoroutine(WaitSpawnEnemy(rndDelay, enemy));    
    }

    IEnumerator WaitSpawnEnemy(float delay, BaseEnemy enemy)
    {
        yield return new WaitForSeconds(delay);

        Debug.Log("Spawning Enemy");

        //find a random position within the spawn circle
        Vector2 randomPointSpawnCircle = (Random.insideUnitCircle.normalized * Random.Range(0, radius)) + new Vector2(transform.position.x, transform.position.z);

        enemy.gameObject.SetActive(true);
        enemy.Refresh(new Vector3(randomPointSpawnCircle.x, 0, randomPointSpawnCircle.y));
        enemy.aiManager.Refresh(this);
    }

    //Spawns a pickup in a random position within the Spawn Area
    public void SpawnPickup()
    {

    }

}

public struct Circle2D
{
    public Vector2 pos;
    public float radius;

    public Circle2D(Vector2 _pos, Vector2 _offset, float _radius)
    {
        pos = _pos + _offset;
        radius = _radius;
    }
}
