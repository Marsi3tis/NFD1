using UnityEngine;

public class DropPickup : MonoBehaviour
{
    public DropSpawnerPoints spawner;
    public Vector3Int spawnPointIndex;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        Debug.Log("Drop picked up!");

        DropData data = GetComponent<DropData>();
        if (data != null)
            InventoryManager.Instance.AddItem(data.itemType);

        // Award XP
        XPReward xpReward = GetComponent<XPReward>();
        if (xpReward != null)
            ExperienceManager.Instance.AddXP(xpReward.xpAmount);

        if (spawner != null)
        {
            spawner.FreeSpawnPoint(spawnPointIndex);
            spawner.SpawnDropAtFreePoint();
        }

        Destroy(gameObject);
        GopnikEvent.Instance.DropIsPicked();
    }
}
