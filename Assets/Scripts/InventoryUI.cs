using UnityEngine;
using TMPro;

public class InventoryUI : MonoBehaviour
{
    public TMP_Text ganjaText;
    public TMP_Text krText;
    public TMP_Text sniegasText;

    private void Update()
    {
        if (InventoryManager.Instance == null) return;

        ganjaText.text  = "Ganja: "  + InventoryManager.Instance.GetCount(ItemType.Ganja);
        krText.text     = "Kr: "     + InventoryManager.Instance.GetCount(ItemType.Kr);
        sniegasText.text = "Sniegas: " + InventoryManager.Instance.GetCount(ItemType.Sniegas);
    }
}
