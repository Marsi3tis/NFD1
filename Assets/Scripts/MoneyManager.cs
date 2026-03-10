using UnityEngine;

public class MoneyManager : MonoBehaviour
{
    public static MoneyManager Instance { get; private set; }

    private float money = 0f;

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
        money += amount;
        Debug.Log($"Money added: {amount}. Total: {money}");
    }

    public bool SpendMoney(float amount)
    {
        if (money < amount)
        {
            Debug.Log("Not enough money.");
            return false;
        }

        money -= amount;
        return true;
    }
}
