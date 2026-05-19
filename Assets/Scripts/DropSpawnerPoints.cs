using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DropSpawnerPoints : MonoBehaviour
{
    public GameObject ganjaPrefab;
    public GameObject krPrefab;
    public GameObject sniegasPrefab;

    [Header("Tilemap Settings")]
    public Tilemap tilemap;
    public List<TileBase> spawnableTiles = new List<TileBase>();
    public int startSpawnCount = 5;
    private Dictionary<Vector3Int, GameObject> occupiedPoints = new Dictionary<Vector3Int, GameObject>();
    private List<Vector3Int> allSpawnTileCoordinates = new List<Vector3Int>();
    private void Start()
    {
        if (tilemap == null || spawnableTiles == null || spawnableTiles.Count == 0)
        {
            Debug.LogError("Tilemap spawn ERROR");
            return;
        }

        FindAllValidTiles();

        for (int i = 0; i < startSpawnCount; i++)
        {
            SpawnDropAtFreePoint();
        }
    }

    public void FindAllValidTiles()
    {
        BoundsInt bounds = tilemap.cellBounds;

        foreach(var pos in bounds.allPositionsWithin)
        {
            TileBase currentTile = tilemap.GetTile(pos);
            if (currentTile != null && spawnableTiles.Contains(currentTile))
            {
                allSpawnTileCoordinates.Add(pos);
            }
        }
    }
    public void SpawnDropAtFreePoint()
    {
        List<Vector3Int> freeTiles = new List<Vector3Int>();

        for (int i = 0; i < allSpawnTileCoordinates.Count; i++)
        {
            Vector3Int tilePos = allSpawnTileCoordinates[i];
            if (!occupiedPoints.ContainsKey(tilePos))
            {
                freeTiles.Add(tilePos);
            }
        }

        if (freeTiles.Count == 0)
        {
            UnityEngine.Debug.Log("No free spawn points left.");
            return;
        }

        int randomListIndex = UnityEngine.Random.Range(0, freeTiles.Count);
        Vector3Int targetTilePos = freeTiles[randomListIndex];
        Vector3 spawnPosition = tilemap.GetCellCenterWorld(targetTilePos);

        GameObject prefab = GetRandomDropByChance();
        GameObject newDrop = Instantiate(prefab, spawnPosition, Quaternion.identity);

        DropPickup pickup = newDrop.GetComponent<DropPickup>();
        if (pickup != null)
        {
            pickup.spawner = this;
            pickup.spawnPointIndex = targetTilePos;
        }

        occupiedPoints.Add(targetTilePos, newDrop);
    }

    public void FreeSpawnPoint(Vector3Int tilePos)
    {
        if (occupiedPoints.ContainsKey(tilePos))
        {
            occupiedPoints.Remove(tilePos);
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