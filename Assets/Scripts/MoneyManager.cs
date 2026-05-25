using UnityEngine;

public class MoneyManager : MonoBehaviour
{
    public static MoneyManager Instance { get; private set; }

    [SerializeField] private float money = 0f;

    public float Money => money;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void AddMoney(float amount)
    {
        if (amount <= 0f)
            return;

        money += amount;
        Debug.Log($"Money added: {amount}. Total: {money}");
    }

    public bool CanAfford(float amount)
    {
        if (amount <= 0f)
            return false;

        return money >= amount;
    }

    public bool SpendMoney(float amount)
    {
        if (amount <= 0f)
        {
            Debug.LogWarning("SpendMoney failed: amount must be higher than 0.");
            return false;
        }

        if (money < amount)
        {
            Debug.Log($"Not enough money. Have: {money}, need: {amount}");
            return false;
        }

        money -= amount;
        Debug.Log($"Money spent: {amount}. Total: {money}");
        return true;
    }
}