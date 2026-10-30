using UnityEngine;

public class DroneSpawner : MonoBehaviour
{
    public GameObject dronePrefab;
    public Transform[] spawnPoints;
    public float spawnInterval = 3f;
    public int maxDrones = 5;

    private float nextSpawnTime;
    private int currentCount = 0;

    void Update()
    {
        if (Time.time >= nextSpawnTime && currentCount < maxDrones)
        {
            nextSpawnTime = Time.time + spawnInterval;
            SpawnDrone();
        }
    }

    void SpawnDrone()
    {
        if (spawnPoints.Length == 0) return;

        Transform point = spawnPoints[Random.Range(0, spawnPoints.Length)];
        Instantiate(dronePrefab, point.position, Quaternion.identity);
        currentCount++;
        Debug.Log("Заспавнен дрон. Всего: " + currentCount);
    }
}
