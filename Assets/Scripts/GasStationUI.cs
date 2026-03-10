using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GasStationUI : MonoBehaviour
{
    [Header("Prompt (rodomas artėjant)")]
    public GameObject promptPanel;
    public TMP_Text promptText;             // "[E] Pildyti degalus - 1.50 EUR/L"

    [Header("Refuel Panel (rodomas pildant)")]
    public GameObject refuelPanel;
    public Slider fuelSlider;              // Degalų lygio juosta
    public TMP_Text fuelText;             // "Degalai: 45.0 / 100.0 L"
    public TMP_Text costText;             // "Balansas: 23.50 EUR"
    public TMP_Text statusText;           // "Pildoma..." arba "Bakas pilnas"

    private void Start()
    {
        // Paslėpti viską iš karto
        if (promptPanel != null) promptPanel.SetActive(false);
        if (refuelPanel != null) refuelPanel.SetActive(false);
    }

    /// <summary>Rodo arba slepia [E] priminimą su kaina</summary>
    public void ShowPrompt(bool show, float pricePerLiter)
    {
        if (promptPanel == null) return;

        promptPanel.SetActive(show);

        if (show && promptText != null)
            promptText.text = $"[E] Pildyti degalus\n{pricePerLiter:F2} EUR/L";
    }

    /// <summary>Rodo arba slepia pildymo panelį</summary>
    public void ShowRefuelPanel(bool show)
    {
        if (refuelPanel == null) return;
        refuelPanel.SetActive(show);

        if (statusText != null)
            statusText.text = show ? "Pildoma..." : "";
    }

    /// <summary>Atnaujina degalų ir pinigų informaciją realiu laiku</summary>
    public void UpdateRefuelInfo(float currentFuel, float maxFuel, float balance)
    {
        if (fuelSlider != null)
        {
            fuelSlider.maxValue = maxFuel;
            fuelSlider.value = currentFuel;
        }

        if (fuelText != null)
            fuelText.text = $"Degalai: {currentFuel:F1} / {maxFuel:F0} L";

        if (costText != null)
            costText.text = $"Balansas: {balance:F2} EUR";

        if (statusText != null)
        {
            if (currentFuel >= maxFuel)
                statusText.text = "✓ Bakas pilnas!";
            else if (balance <= 0f)
                statusText.text = "✗ Nepakanka pinigų!";
            else
                statusText.text = "Pildoma...";
        }
    }
}
