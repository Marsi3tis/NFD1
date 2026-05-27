using UnityEngine;

public class FuelSystem : MonoBehaviour
{
    [Header("Game Over Manager")]
    [SerializeField] private GameOverManager gameOverManager;
    [Header("Fuel")]
    public float maxFuel = 100f;
    public float currentFuel = 100f;
    public float fuelBurnPerSecond = 4f;
    public float idleBurnPerSecond = 0f;

    [Header("Movement Script")]
    public TopControl movementScript;

    private Rigidbody2D rb;
    private bool fuelEmptyLogged = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        currentFuel = maxFuel;
    }

    private void Update()
    {
        float speed = 0f;

        if (rb != null)
            speed = rb.linearVelocity.magnitude;

        if (currentFuel > 0f)
        {
            if (speed > 0.05f)
                currentFuel -= fuelBurnPerSecond * Time.deltaTime;
            else
                currentFuel -= idleBurnPerSecond * Time.deltaTime;

            currentFuel = Mathf.Clamp(currentFuel, 0f, maxFuel);
        }

        if (movementScript != null)
            movementScript.enabled = currentFuel > 0f;

        if (currentFuel <= 0f && !fuelEmptyLogged)
        {
            fuelEmptyLogged = true;
            UnityEngine.Debug.Log("Fuel empty!");

            if (rb != null)
                rb.linearVelocity = Vector2.zero;
            
            GameOverManager manager = gameOverManager != null ? gameOverManager : GameOverManager.Instance;
            if (manager != null)
                manager.ShowFuelEmptyGameOver();
            else
                Debug.LogError("GameOverManager not found! Please assign it in the inspector or ensure it exists in the scene.");
        }
    }

    public void AddFuel(float amount)
    {
        currentFuel = Mathf.Clamp(currentFuel + amount, 0f, maxFuel);
        fuelEmptyLogged = false;

        if (movementScript != null && currentFuel > 0f)
            movementScript.enabled = true;
    }
    public void ResetFuel()
    {
        currentFuel = maxFuel;
        fuelEmptyLogged = false;

        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }
        if (movementScript != null)
            movementScript.enabled = true;
    }
}