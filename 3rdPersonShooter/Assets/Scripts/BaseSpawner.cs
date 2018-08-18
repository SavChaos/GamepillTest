using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseSpawner : MonoBehaviour
{
    public float radius = 10;
    public float zOffset = 0;
    public float xOffset = 0;
    public Circle2D spawnBounds;

    private void Awake()
    {
        spawnBounds = new Circle2D(new Vector2(transform.position.x, transform.position.z), radius);
    }

    //Spawns an enemy in a random position within the Spawn Area
    public void SpawnEnemy(BaseEnemy enemy)
    {
        Debug.Log("Spawning Enemy");

        //find a random position within the spawn circle
        Vector2 randomPointSpawnCircle = (Random.insideUnitCircle.normalized * Random.Range(0, radius)) + new Vector2(transform.position.x, transform.position.z);

        enemy.Refresh(new Vector3(randomPointSpawnCircle.x, 0, randomPointSpawnCircle.y));
        enemy.aiManager.Refresh(this);
    }

    //Spawns a pickup in a random position within the Spawn Area
    public void SpawnPickup()
    {

    }

}

public class Circle2D
{
    public Vector2 pos;
    public float radius;

    public Circle2D(Vector2 _pos, float _radius)
    {
        pos = _pos;
        radius = _radius;
    }
}
