using UnityEngine;

public class DropPickup : MonoBehaviour
{
    public DropSpawnerPoints spawner;
    public Vector3Int spawnPointIndex;

    [Header("Drop Pickup")]
    [SerializeField] private float maxPickupSpeed = 0.15f;
    private bool pickedUp;
    private void OnTriggerStay2D(Collider2D other)
    {
        if (pickedUp)
            return;

        if (!other.CompareTag("Player"))
            return;

        Rigidbody2D playerRb = other.attachedRigidbody;
        if (playerRb == null)
            return;
        
        if(playerRb == null)
            return;

        float playerSpeed = playerRb.linearVelocity.magnitude;    

        if (playerSpeed > maxPickupSpeed)
        {
            Debug.Log("Player is moving too fast to pick up the drop." + playerSpeed);
            return;
        }
        PickUpDrop();
    }
    private void PickUpDrop()
    {
        pickedUp = true;
        Debug.Log("Drop picked up!");

        DropData data = GetComponent<DropData>();
        if (data != null && InventoryManager.Instance != null)
            InventoryManager.Instance.AddItem(data.itemType);

        // Award XP
        XPReward xpReward = GetComponent<XPReward>();
        if (xpReward != null && ExperienceManager.Instance != null)
            ExperienceManager.Instance.AddXP(xpReward.xpAmount);

        if (spawner != null)
        {
            spawner.FreeSpawnPoint(spawnPointIndex);
            spawner.SpawnDropAtFreePoint();
        }
        if(DropRandomEventManager.Instance != null)
        {
            DropRandomEventManager.Instance.DropIsPicked();
        }
        Destroy(gameObject);
    }
}
