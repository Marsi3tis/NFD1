using UnityEngine;
using TMPro;

public class MoneyUI : MonoBehaviour
{
    public TMP_Text moneyText;

    private void Update()
    {
        if (MoneyManager.Instance == null) return;
        moneyText.text = "EUR " + MoneyManager.Instance.Money.ToString("F2");
    }
}
