using UnityEngine;

public class DropPickup : MonoBehaviour
{
    public DropSpawnerPoints spawner;
    public int spawnPointIndex;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        Debug.Log("Drop picked up!");

        DropData data = GetComponent<DropData>();
        if (data != null)
            InventoryManager.Instance.AddItem(data.itemType);

        if (spawner != null)
        {
            spawner.FreeSpawnPoint(spawnPointIndex);
            spawner.SpawnDropAtFreePoint();
        }

        Destroy(gameObject);
    }
}
