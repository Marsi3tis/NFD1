using UnityEngine;

public class PlayerSkills : MonoBehaviour
{
    public static PlayerSkills Instance { get; private set; }

    [Header("Skill Levels")]
    [SerializeField] private int mahalkesLevel = 0;
    [SerializeField] private int pisiZaibaLevel = 0;
    [SerializeField] private int vierhaturaLevel = 0;

    [Header("Max Skill Levels")]
    public int maxMahalkesLevel = 5;
    public int maxPisiZaibaLevel = 5;
    public int maxVierhaturaLevel = 1;

    [Header("Chance Bonus Per Point")]
    public float mahalkesBonusPerPoint = 0.10f;
    public float pisiZaibaBonusPerPoint = 0.12f;

    [Header("Vierhatura Requirement")]
    public int vierhaturaMinLevel = 15; // Monstras
    public int vierhaturaMaxLevel = 19; // Dievas

    public int MahalkesLevel => mahalkesLevel;
    public int PisiZaibaLevel => pisiZaibaLevel;
    public int VierhaturaLevel => vierhaturaLevel;

    public bool HasVierhatura => vierhaturaLevel > 0;

    public bool CanUseVierhatura
    {
        get
        {
            if (!HasVierhatura)
                return false;

            if (ExperienceManager.Instance == null)
                return false;

            int currentLevel = ExperienceManager.Instance.CurrentLevel;

            return currentLevel >= vierhaturaMinLevel &&
                   currentLevel <= vierhaturaMaxLevel;
        }
    }

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

    public float GetMahalkesBonus()
    {
        return mahalkesLevel * mahalkesBonusPerPoint;
    }

    public float GetPisiZaibaBonus()
    {
        return pisiZaibaLevel * pisiZaibaBonusPerPoint;
    }

    public void UpgradeMahalkes()
    {
        if (mahalkesLevel >= maxMahalkesLevel)
        {
            UnityEngine.Debug.Log("Mahalkes already maxed.");
            return;
        }

        if (ExperienceManager.Instance == null)
        {
            UnityEngine.Debug.Log("ExperienceManager not found.");
            return;
        }

        if (!ExperienceManager.Instance.SpendSkillPoint(1))
        {
            UnityEngine.Debug.Log("Not enough skill points.");
            return;
        }

        mahalkesLevel++;
        UnityEngine.Debug.Log("Mahalkes upgraded to level " + mahalkesLevel);
    }

    public void UpgradePisiZaiba()
    {
        if (pisiZaibaLevel >= maxPisiZaibaLevel)
        {
            UnityEngine.Debug.Log("Pisi žaibą already maxed.");
            return;
        }

        if (ExperienceManager.Instance == null)
        {
            UnityEngine.Debug.Log("ExperienceManager not found.");
            return;
        }

        if (!ExperienceManager.Instance.SpendSkillPoint(1))
        {
            UnityEngine.Debug.Log("Not enough skill points.");
            return;
        }

        pisiZaibaLevel++;
        UnityEngine.Debug.Log("Pisi žaibą upgraded to level " + pisiZaibaLevel);
    }

    public void UpgradeVierhatura()
    {
        if (vierhaturaLevel >= maxVierhaturaLevel)
        {
            UnityEngine.Debug.Log("Vierhatura already unlocked.");
            return;
        }

        if (ExperienceManager.Instance == null)
        {
            UnityEngine.Debug.Log("ExperienceManager not found.");
            return;
        }

        int currentLevel = ExperienceManager.Instance.CurrentLevel;

        if (currentLevel < vierhaturaMinLevel || currentLevel > vierhaturaMaxLevel)
        {
            UnityEngine.Debug.Log("Vierhatura can only be unlocked from Monstras to Dievas.");
            return;
        }

        if (!ExperienceManager.Instance.SpendSkillPoint(1))
        {
            UnityEngine.Debug.Log("Not enough skill points.");
            return;
        }

        vierhaturaLevel++;
        UnityEngine.Debug.Log("Vierhatura unlocked.");
    }
}