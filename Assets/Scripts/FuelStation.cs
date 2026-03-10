using UnityEngine;
using TMPro;

public class FuelStation : MonoBehaviour
{
    [Header("Fuel Settings")]
    public float refuelPerSecond = 20f;
    public float costPerSecond = 10f;
    public float standStillSpeed = 0.05f;

    [Header("UI")]
    public TMP_Text promptText;

    private FuelSystem currentFuelSystem;
    private Rigidbody2D currentRb;
    private bool playerInside = false;

    private void Start()
    {
        if (promptText != null)
            promptText.gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        currentFuelSystem = other.GetComponent<FuelSystem>();
        currentRb = other.GetComponent<Rigidbody2D>();
        playerInside = true;

        RefreshPrompt();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        currentFuelSystem = null;
        currentRb = null;
        playerInside = false;

        if (promptText != null)
            promptText.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (!playerInside || currentFuelSystem == null || currentRb == null)
            return;

        RefreshPrompt();

        float speed = currentRb.linearVelocity.magnitude;
        bool standingStill = speed <= standStillSpeed;
        bool fuelNotFull = currentFuelSystem.currentFuel < currentFuelSystem.maxFuel;

        if (standingStill && fuelNotFull && Input.GetKey(KeyCode.E))
        {
            float moneyCostThisFrame = costPerSecond * Time.deltaTime;
            float fuelThisFrame = refuelPerSecond * Time.deltaTime;

            if (MoneyManager.Instance != null && MoneyManager.Instance.SpendMoney(moneyCostThisFrame))
            {
                currentFuelSystem.AddFuel(fuelThisFrame);
            }
        }
    }

    private void RefreshPrompt()
    {
        if (promptText == null || currentFuelSystem == null || currentRb == null)
            return;

        promptText.gameObject.SetActive(true);

        float speed = currentRb.linearVelocity.magnitude;
        bool standingStill = speed <= standStillSpeed;
        bool fuelNotFull = currentFuelSystem.currentFuel < currentFuelSystem.maxFuel;

        if (!fuelNotFull)
        {
            promptText.text = "Bakas pilnas";
            return;
        }

        if (!standingStill)
        {
            promptText.text = "Sustok, kad piltum kura";
            return;
        }

        if (MoneyManager.Instance == null)
        {
            promptText.text = "Nera MoneyManager";
            return;
        }

        if (MoneyManager.Instance.Money <= 0f)
        {
            promptText.text = "Nepakanka pinigu";
            return;
        }

        promptText.text = "Spausk E kuro pylimui";
    }
}