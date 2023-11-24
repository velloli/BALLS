using UnityEngine;

public class ObjectPlacer : MonoBehaviour
{

    void Start()
    {
        PlaceObjectOnTerrain();
    }

    void PlaceObjectOnTerrain()
    {
        Terrain terrain = Terrain.activeTerrain;
        float terrainWidth = terrain.terrainData.size.x;
        float terrainHeight = terrain.terrainData.size.z;

        float randomX = Random.Range(0f, terrainWidth);
        float randomY = Random.Range(0f, terrainHeight);

        float terrainHeightAtPosition = terrain.SampleHeight(new Vector3(randomX, 0, randomY));

        Vector3 randomPosition = new Vector3(randomX, terrainHeightAtPosition, randomY);
        randomPosition.y += 1;

        gameObject.transform.position = randomPosition;
    }
}