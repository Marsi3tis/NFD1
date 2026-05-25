using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FuelBarUI : MonoBehaviour
{
    [Header("Reikalingi komponentai")]
    public FuelSystem fuelSystem;       // Vilk automobilio objektą čia

    [Header("UI Elementai")]
    public Slider fuelSlider;           // Fuel baras
    public Image fillImage;             // Slider > Fill Area > Fill (Image)
    public TMP_Text fuelLabel;          // Tekstas, pvz: "FUEL  72 / 100"

    [Header("Spalvos pagal lygį")]
    public Color colorFull    = new Color(0.2f, 0.85f, 0.3f);   // Žalia
    public Color colorMedium  = new Color(1f,   0.75f, 0f);     // Geltona
    public Color colorLow     = new Color(1f,   0.25f, 0.1f);   // Raudona

    [Header("Animacija")]
    public float lowFuelThreshold   = 25f;   // % – žemiau šio lygio mirksi
    public float blinkSpeed         = 2.5f;

    private float blinkTimer = 0f;
    private bool  isBlinking = false;

    private void Update()
    {
        if (fuelSystem == null) return;

        float current = fuelSystem.currentFuel;
        float max     = fuelSystem.maxFuel;
        float percent = max > 0f ? current / max : 0f;

        // --- Slider ---
        if (fuelSlider != null)
        {
            fuelSlider.minValue = 0f;
            fuelSlider.maxValue = max;
            fuelSlider.value    = current;
        }

        // --- Spalva ---
        if (fillImage != null)
        {
            Color targetColor;
            if (percent > 0.5f)
                targetColor = Color.Lerp(colorMedium, colorFull, (percent - 0.5f) * 2f);
            else
                targetColor = Color.Lerp(colorLow, colorMedium, percent * 2f);

            fillImage.color = targetColor;
        }

        // --- Tekstas ---
        if (fuelLabel != null)
            fuelLabel.text = $"FUEL  {current:F0}/{max:F0}";

        // --- Mirksi kai mažai ---
        isBlinking = percent * 100f <= lowFuelThreshold;

        if (isBlinking && fuelSlider != null)
        {
            blinkTimer += Time.deltaTime * blinkSpeed;
            float alpha = Mathf.Abs(Mathf.Sin(blinkTimer * Mathf.PI));

            Color c = fillImage.color;
            c.a = Mathf.Lerp(0.25f, 1f, alpha);
            fillImage.color = c;
        }
        else
        {
            blinkTimer = 0f;

            if (fillImage != null)
            {
                Color c = fillImage.color;
                c.a = 1f;
                fillImage.color = c;
            }
        }
    }
}
