using UnityEngine;

public class DropPickup : MonoBehaviour
{
    public DropSpawnerPoints spawner;
    public int spawnPointIndex;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        UnityEngine.Debug.Log("Drop picked up!");

        if (spawner != null)
        {
            spawner.FreeSpawnPoint(spawnPointIndex);
            spawner.SpawnDropAtFreePoint();
        }

        Destroy(gameObject);
    }
}