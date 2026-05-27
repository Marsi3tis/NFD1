using UnityEngine;
using UnityEngine.Events;

public class ExperienceManager : MonoBehaviour
{
    public static ExperienceManager Instance { get; private set; }

    [Header("Leveling Settings")]
    public int baseXPPerLevel = 100;
    public float levelScaling = 1.5f;
    public int skillPointsPerLevel = 1;

    [Header("Player Progress")]
    [SerializeField] private int currentXP = 0;
    [SerializeField] private int currentLevel = 1;
    [SerializeField] private int skillPoints = 0;

    private readonly string[] rankNames =
    {
    "Šiukšlė",
    "Išmataveidis",
    "Gandonas",
    "Čmo",
    "Pituhas",
    "Urodas",
    "Vaflis",
    "Lavonas",
    "Padugnė",
    "Dušnyla",
    "Mudakas",
    "Plotnekas",
    "Čiortas",
    "Spektras",
    "Monstras",
    "Žvėris",
    "Legenda",
    "Caras",
    "Dievas"
    };

    public int CurrentXP => currentXP;
    public int CurrentLevel => currentLevel;
    public int SkillPoints => skillPoints;
    public string CurrentRank => GetRankName(currentLevel);

    public UnityEvent<int> OnLevelUp = new UnityEvent<int>();
    public UnityEvent<int> OnSkillPointsChanged = new UnityEvent<int>();
    public UnityEvent<string> OnRankChanged = new UnityEvent<string>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
       // DontDestroyOnLoad(gameObject);
    }

    public void AddXP(int amount)
    {
        if (amount <= 0)
            return;

        currentXP += amount;

        Debug.Log($"Gained {amount} XP. Total XP: {currentXP}");

        CheckLevelUp();
    }

    public void RemoveXP(int amount)
    {
        if (amount <= 0)
            return;

        currentXP = Mathf.Max(0, currentXP - amount);

        Debug.Log($"Removed {amount} XP. Total XP: {currentXP}");
    }

    public int GetXPForNextLevel()
    {
        return Mathf.RoundToInt(baseXPPerLevel * Mathf.Pow(levelScaling, currentLevel - 1));
    }

    public float GetLevelProgress()
    {
        int xpForNextLevel = GetXPForNextLevel();

        if (xpForNextLevel <= 0)
            return 0f;

        return Mathf.Clamp01((float)currentXP / xpForNextLevel);
    }

    public bool SpendSkillPoint(int amount = 1)
    {
        if (amount <= 0)
            return false;

        if (skillPoints < amount)
            return false;

        skillPoints -= amount;
        OnSkillPointsChanged.Invoke(skillPoints);

        Debug.Log($"Spent {amount} skill point(s). Remaining: {skillPoints}");

        return true;
    }

    public void AddSkillPoints(int amount)
    {
        if (amount <= 0)
            return;

        skillPoints += amount;
        OnSkillPointsChanged.Invoke(skillPoints);

        Debug.Log($"Added {amount} skill point(s). Total: {skillPoints}");
    }

    private void CheckLevelUp()
    {
        while (currentXP >= GetXPForNextLevel())
        {
            currentXP -= GetXPForNextLevel();
            currentLevel++;

            AddSkillPoints(skillPointsPerLevel);

            Debug.Log($"Level up! Now level {currentLevel}. Rank: {CurrentRank}");

            OnLevelUp.Invoke(currentLevel);
            OnRankChanged.Invoke(CurrentRank);
        }
    }

    private string GetRankName(int level)
    {
        int rankIndex = Mathf.Clamp(level - 1, 0, rankNames.Length - 1);
        return rankNames[rankIndex];
    }
    public void ResetProgress()
    {
        currentXP = 0;
        currentLevel = 1;
        skillPoints = 0;

        OnSkillPointsChanged.Invoke(skillPoints);
        OnRankChanged.Invoke(CurrentRank);
    }
}