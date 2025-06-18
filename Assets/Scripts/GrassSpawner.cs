using UnityEngine;

public class GrassSpawner : MonoBehaviour
{
    public GameObject grassPrefab; // Prefab for the grass object
    public int numberOfGrassPatches = 100; // Number of grass patches to spawn

    public Vector3 areaSize = new Vector3(10, 0, 10); // Size of the area to spawn grass patches
    void Start()
    {
        SpawnGrass();
    }

    void SpawnGrass()
    {
        for (int i = 0; i < numberOfGrassPatches; i++)
        {
            Vector3 randomPos = new Vector3(
                Random.Range(-areaSize.x / 2f, areaSize.x / 2f),
                0f,
                Random.Range(-areaSize.z / 2f, areaSize.z / 2f)
            );

            Vector3 spawnPos = transform.position + randomPos;

            Instantiate(grassPrefab, spawnPos, Quaternion.Euler(0, Random.Range(0, 360f), 0), transform);
        }
    }
}

