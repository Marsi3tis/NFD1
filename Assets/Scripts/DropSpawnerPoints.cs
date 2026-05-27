using System.Collections.Generic;
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
        ResetSpawner();
    }

    public void ResetSpawner()
    {
        ClearExistingDrops();

        occupiedPoints.Clear();
        allSpawnTileCoordinates.Clear();

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

    private void ClearExistingDrops()
    {
        foreach (GameObject drop in occupiedPoints.Values)
        {
            if (drop != null)
                Destroy(drop);
        }
    }

    public void FindAllValidTiles()
    {
        BoundsInt bounds = tilemap.cellBounds;

        foreach (var pos in bounds.allPositionsWithin)
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
            Debug.Log("No free spawn points left.");
            return;
        }

        int randomListIndex = Random.Range(0, freeTiles.Count);
        Vector3Int targetTilePos = freeTiles[randomListIndex];
        Vector3 spawnPosition = tilemap.GetCellCenterWorld(targetTilePos);

        GameObject prefab = GetRandomDropByChance();

        if (prefab == null)
        {
            Debug.LogError("Drop prefab is missing.");
            return;
        }

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
        int roll = Random.Range(0, 100);

        if (roll < 60) return ganjaPrefab;
        if (roll < 90) return krPrefab;
        return sniegasPrefab;
    }
}