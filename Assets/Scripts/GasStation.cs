using UnityEngine;

public class GasStation : MonoBehaviour
{
    [Header("Fuel Settings")]
    public float pricePerLiter = 1.5f;       // EUR už litrą
    public float fuelPerSecond = 20f;         // Kiek litrų per sekundę pripilama
    public float maxFuelCapacity = 100f;      // Degalinės talpa (neprivaloma)

    [Header("UI")]
    public GasStationUI stationUI;            // Priskirk GasStationUI objektą

    private FuelSystem playerFuel;
    private bool isRefueling = false;
    private bool playerInRange = false;

    private void Update()
    {
        if (!playerInRange || playerFuel == null) return;

        // Pradėti/sustabdyti pildymą paspaudus E
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (!isRefueling)
                StartRefueling();
            else
                StopRefueling();
        }

        // Aktyvus pildymas
        if (isRefueling)
            RefuelTick();
    }

    private void RefuelTick()
    {
        float fuelNeeded = playerFuel.maxFuel - playerFuel.currentFuel;

        if (fuelNeeded <= 0f)
        {
            StopRefueling();
            return;
        }

        float fuelToAdd = fuelPerSecond * Time.deltaTime;
        fuelToAdd = Mathf.Min(fuelToAdd, fuelNeeded); // Nepildyti daugiau nei reikia

        float cost = fuelToAdd * pricePerLiter;

        // Bandyti nuskaičiuoti pinigus
        if (!MoneyManager.Instance.SpendMoney(cost))
        {
            // Neužtenka pinigų — pildyti tik tiek, kiek galima
            float maxAffordableFuel = MoneyManager.Instance.Money / pricePerLiter;
            if (maxAffordableFuel <= 0f)
            {
                Debug.Log("Neužtenka pinigų degalams!");
                StopRefueling();
                return;
            }

            fuelToAdd = maxAffordableFuel;
            cost = fuelToAdd * pricePerLiter;
            MoneyManager.Instance.SpendMoney(cost);
        }

        playerFuel.AddFuel(fuelToAdd);

        if (stationUI != null)
            stationUI.UpdateRefuelInfo(playerFuel.currentFuel, playerFuel.maxFuel, MoneyManager.Instance.Money);
    }

    private void StartRefueling()
    {
        isRefueling = true;
        Debug.Log("Pradedamas degalų pildymas...");

        if (stationUI != null)
            stationUI.ShowRefuelPanel(true);
    }

    private void StopRefueling()
    {
        isRefueling = false;
        Debug.Log("Degalų pildymas sustabdytas.");

        if (stationUI != null)
            stationUI.ShowRefuelPanel(false);
    }

    // --- Trigger zona ---
    private void OnTriggerEnter2D(Collider2D other)
    {
        FuelSystem fuel = other.GetComponent<FuelSystem>();
        if (fuel == null) return;

        playerFuel = fuel;
        playerInRange = true;

        if (stationUI != null)
            stationUI.ShowPrompt(true, pricePerLiter);

        Debug.Log("Įvažiavai į degalinę. Spausk [E] pildyti degalus.");
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.GetComponent<FuelSystem>() == null) return;

        StopRefueling();
        playerFuel = null;
        playerInRange = false;

        if (stationUI != null)
        {
            stationUI.ShowPrompt(false, 0f);
            stationUI.ShowRefuelPanel(false);
        }

        Debug.Log("Išvažiavai iš degalinės.");
    }
}
