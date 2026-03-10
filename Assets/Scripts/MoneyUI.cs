using UnityEngine;
using TMPro;

public class MoneyUI : MonoBehaviour
{
    public TMP_Text MoneyText;

    private void Update()
    {
        if (MoneyManager.Instance == null) return;
        MoneyText.text = "EUR" + MoneyManager.Instance.Money.ToString("F2");
    }
}
