using System.Collections.Generic;
using UnityEngine;

public class DropSpawnerPoints : MonoBehaviour
{
    public GameObject ganjaPrefab;
    public GameObject krPrefab;
    public GameObject sniegasPrefab;

    public Transform[] spawnPoints;
    public int startSpawnCount = 5;

    private Dictionary<int, GameObject> occupiedPoints = new Dictionary<int, GameObject>();

    private void Start()
    {
        for (int i = 0; i < startSpawnCount; i++)
        {
            SpawnDropAtFreePoint();
        }
    }

    public void SpawnDropAtFreePoint()
    {
        List<int> freeIndexes = new List<int>();

        for (int i = 0; i < spawnPoints.Length; i++)
        {
            if (!occupiedPoints.ContainsKey(i))
            {
                freeIndexes.Add(i);
            }
        }

        if (freeIndexes.Count == 0)
        {
            UnityEngine.Debug.Log("No free spawn points left.");
            return;
        }

        int randomListIndex = UnityEngine.Random.Range(0, freeIndexes.Count);
        int spawnIndex = freeIndexes[randomListIndex];

        GameObject prefab = GetRandomDropByChance();
        GameObject newDrop = Instantiate(prefab, spawnPoints[spawnIndex].position, Quaternion.identity);

        DropPickup pickup = newDrop.GetComponent<DropPickup>();
        if (pickup != null)
        {
            pickup.spawner = this;
            pickup.spawnPointIndex = spawnIndex;
        }

        occupiedPoints.Add(spawnIndex, newDrop);
    }

    public void FreeSpawnPoint(int spawnIndex)
    {
        if (occupiedPoints.ContainsKey(spawnIndex))
        {
            occupiedPoints.Remove(spawnIndex);
        }
    }

    private GameObject GetRandomDropByChance()
    {
        int roll = UnityEngine.Random.Range(0, 100);

        if (roll < 60) return ganjaPrefab;
        if (roll < 90) return krPrefab;
        return sniegasPrefab;
    }
}