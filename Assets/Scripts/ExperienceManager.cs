using UnityEngine;
using UnityEngine.Events;

public class ExperienceManager : MonoBehaviour
{
    public static ExperienceManager Instance { get; private set; }

    [Header("Leveling Settings")]
    public int baseXPPerLevel = 100;  // XP needed for level 1 -> 2
    public float levelScaling = 1.5f; // multiplier per level

    private int currentXP = 0;
    private int currentLevel = 1;

    public int CurrentXP    => currentXP;
    public int CurrentLevel => currentLevel;

    // Subscribe to this from UI or other systems to react to level ups
    public UnityEvent<int> OnLevelUp = new UnityEvent<int>();

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

    public void AddXP(int amount)
    {
        currentXP += amount;
        Debug.Log($"Gained {amount} XP. Total: {currentXP}");

        CheckLevelUp();
    }
    public void RemoveXP(int amount)
    {
        currentXP -= amount;
        Debug.Log($"Lost {amount} XP. Total: {currentXP}");

        CheckLevelUp();
    }

    public int GetXPForNextLevel()
    {
        return Mathf.RoundToInt(baseXPPerLevel * Mathf.Pow(levelScaling, currentLevel - 1));
    }

    public float GetLevelProgress()
    {
        return (float)currentXP / GetXPForNextLevel();
    }

    private void CheckLevelUp()
    {
        while (currentXP >= GetXPForNextLevel())
        {
            currentXP -= GetXPForNextLevel();
            currentLevel++;
            Debug.Log($"Level up! Now level {currentLevel}");
            OnLevelUp.Invoke(currentLevel);
        }
    }
}
