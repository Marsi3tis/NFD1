using System.Xml.Serialization;
using UnityEngine;

public class DropPickup : MonoBehaviour
{
    GopnikEvent gopnikEvent;
    public DropSpawnerPoints spawner;
    public int spawnPointIndex;
    void Awake()
    {
        gopnikEvent = GetComponent<GopnikEvent>();
    }
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
        GopnikEvent.Instance.DropIsPicked();
        
        Destroy(gameObject);
    }

}
