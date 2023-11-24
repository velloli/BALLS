using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemySpawner : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject prefab1;
    public GameObject prefab2;
    public float spawnDelay = 1.0f;
    public float spawnRadius = 5.0f;
    public int maxSpawnCount = 10; // Maximum number of prefabs to spawn

    private int spawnCount = 0; // Current number of prefabs spawned
    private Vector3 cubeSize; // Size of the cube to spawn objects in

    void Start()
    {
        // Call the SpawnPrefab function repeatedly with a delay
        cubeSize.x = transform.localScale.x;
        cubeSize.y = transform.localScale.y;
        cubeSize.z = transform.localScale.z;

        InvokeRepeating("SpawnPrefab", spawnDelay, spawnDelay);

    }

    void SpawnPrefab()
    {
        // Spawn only if we haven't reached the maximum spawn count
        if (spawnCount < maxSpawnCount)
        {
            // Choose a random position within the cube
            Vector3 spawnPos = transform.position + new Vector3(
                Random.Range(-cubeSize.x / 2f, cubeSize.x / 2f),
                Random.Range(-cubeSize.y / 2f, cubeSize.y / 2f),
                Random.Range(-cubeSize.z / 2f, cubeSize.z / 2f));

            // Choose a random prefab to spawn
            GameObject prefabToSpawn = Random.value < 0.5f ? prefab1 : prefab2;
            bool grounded = Random.value < 0.2f ? true : false;

            if (grounded)
                spawnPos.y = 0;

            // Spawn the chosen prefab at the chosen position
            Instantiate(prefabToSpawn, spawnPos, Quaternion.identity);

            // Increment the spawn count
            spawnCount++;
        }
        else
        {
            // If we've reached the maximum spawn count, stop spawning
            CancelInvoke("SpawnPrefab");
        }
    }
}
