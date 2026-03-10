using UnityEngine;

public class SalePoint : MonoBehaviour
{
    [Header("Prices per item")]
    public float ganjaPrice   = 10f;
    public float krPrice      = 25f;
    public float sniegasPrice = 50f;

    [Header("Interaction Key")]
    public KeyCode interactKey = KeyCode.E;

    private bool playerInZone = false;

    private void Update()
    {
        if (playerInZone && Input.GetKeyDown(interactKey))
        {
            SaleMenuUI.Instance.Open(this);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInZone = true;
            SaleMenuUI.Instance.ShowPrompt(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInZone = false;
            SaleMenuUI.Instance.ShowPrompt(false);
            SaleMenuUI.Instance.Close();
        }
    }

    public float GetPrice(ItemType type)
    {
        switch (type)
        {
            case ItemType.Ganja:   return ganjaPrice;
            case ItemType.Kr:      return krPrice;
            case ItemType.Sniegas: return sniegasPrice;
            default: return 0f;
        }
    }

    public void SellItem(ItemType type, int amount)
    {
        if (InventoryManager.Instance.GetCount(type) < amount) return;

        float total = GetPrice(type) * amount;
        InventoryManager.Instance.RemoveItem(type, amount);
        MoneyManager.Instance.AddMoney(total);

        Debug.Log($"Sold {amount}x {type} for ${total}");
    }
}
