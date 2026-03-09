using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }

    private Dictionary<ItemType, int> inventory = new Dictionary<ItemType, int>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        foreach (ItemType type in System.Enum.GetValues(typeof(ItemType)))
            inventory[type] = 0;
    }

    public void AddItem(ItemType type, int amount = 1)
    {
        inventory[type] += amount;
        Debug.Log($"Picked up {type}. Total: {inventory[type]}");
    }

    public bool RemoveItem(ItemType type, int amount = 1)
    {
        if (inventory[type] < amount)
        {
            Debug.Log($"Not enough {type} in inventory.");
            return false;
        }

        inventory[type] -= amount;
        return true;
    }

    public int GetCount(ItemType type) => inventory[type];

    public Dictionary<ItemType, int> GetAllItems() => new Dictionary<ItemType, int>(inventory);
}
