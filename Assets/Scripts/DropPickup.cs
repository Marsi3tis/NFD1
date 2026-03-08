using UnityEngine;

public class DropPickup : MonoBehaviour
{
    public DropSpawnerPoints spawner;
    public int spawnPointIndex;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        if (spawner != null)
        {
            spawner.FreeSpawnPoint(spawnPointIndex);
            spawner.SpawnDropAtFreePoint();
        }

        Destroy(gameObject);
    }
}