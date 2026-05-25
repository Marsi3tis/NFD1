using UnityEngine;

public class DropRandomEventManager : MonoBehaviour
{
    public static DropRandomEventManager Instance { get; private set; }

    [Header("Event References")]
    [SerializeField] private GopnikEvent gopnikEvent;
    [SerializeField] private PoliceRaidEvent policeRaidEvent;

    [Header("Main Event Chance")]
    [Tooltip("Chance that ANY random event happens after picking up a drop.")]
    [SerializeField, Range(0f, 1f)] private float totalEventChance = 0.30f;

    [Header("Event Weights")]
    [Tooltip("Higher value means gopnik event is more likely compared to police raid.")]
    [SerializeField, Min(0f)] private float gopnikWeight = 2f;

    [Tooltip("Higher value means police raid is more likely compared to gopnik event.")]
    [SerializeField, Min(0f)] private float policeRaidWeight = 1f;

    [Header("Debug")]
    [SerializeField] private bool showDebugLogs = true;
    [SerializeField] private bool enableDebugKeys = true;
    [SerializeField] private KeyCode forceGopnikKey = KeyCode.F9;
    [SerializeField] private KeyCode forcePoliceKey = KeyCode.F8;

    [Header("Chance Preview")]
    [SerializeField, TextArea(3, 6)] private string chancePreview;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.LogWarning("Duplicate DropRandomEventManager found. Destroying duplicate.");
            Destroy(gameObject);
            return;
        }

        Instance = this;
        UpdateChancePreview();
    }

    private void Update()
    {
        if (!enableDebugKeys)
            return;

        if (Input.GetKeyDown(forceGopnikKey))
        {
            Debug.Log("DEBUG: Force starting Gopnik event.");
            ForceStartGopnikEvent();
        }

        if (Input.GetKeyDown(forcePoliceKey))
        {
            Debug.Log("DEBUG: Force starting Police Raid event.");
            ForceStartPoliceRaidEvent();
        }
    }

    private void OnValidate()
    {
        totalEventChance = Mathf.Clamp01(totalEventChance);
        gopnikWeight = Mathf.Max(0f, gopnikWeight);
        policeRaidWeight = Mathf.Max(0f, policeRaidWeight);

        UpdateChancePreview();
    }

    public void DropIsPicked()
    {
        if (IsAnyEventRunning())
        {
            Log("Drop picked, but an event is already running.");
            return;
        }

        float eventRoll = Random.Range(0f, 1f);

        Log("Drop picked. Event roll: " + eventRoll + " / Total event chance: " + totalEventChance);

        if (eventRoll > totalEventChance)
        {
            Log("No random event happened.");
            return;
        }

        StartWeightedRandomEvent();
    }

    private void StartWeightedRandomEvent()
    {
        float activeGopnikWeight = GetActiveGopnikWeight();
        float activePoliceWeight = GetActivePoliceRaidWeight();

        float totalWeight = activeGopnikWeight + activePoliceWeight;

        if (totalWeight <= 0f)
        {
            Debug.LogWarning("Random event rolled, but no valid event is available.");
            return;
        }

        float roll = Random.Range(0f, totalWeight);

        Log("Weighted event roll: " + roll + " / Total weight: " + totalWeight);

        if (roll < activeGopnikWeight)
        {
            StartGopnikEvent();
        }
        else
        {
            StartPoliceRaidEvent();
        }
    }

    private float GetActiveGopnikWeight()
    {
        if (gopnikEvent == null)
            return 0f;

        if (gopnikEvent.IsEventRunning)
            return 0f;

        return gopnikWeight;
    }

    private float GetActivePoliceRaidWeight()
    {
        if (policeRaidEvent == null)
            return 0f;

        if (policeRaidEvent.IsEventRunning)
            return 0f;

        return policeRaidWeight;
    }

    private void StartGopnikEvent()
    {
        if (gopnikEvent == null)
        {
            Debug.LogWarning("Cannot start Gopnik event. GopnikEvent reference is missing.");
            return;
        }

        bool started = gopnikEvent.StartEventForced();

        if (started)
        {
            Log("Gopnik event started.");
        }
        else
        {
            Log("Gopnik event did not start.");
        }
    }

    private void StartPoliceRaidEvent()
    {
        if (policeRaidEvent == null)
        {
            Debug.LogWarning("Cannot start Police Raid event. PoliceRaidEvent reference is missing.");
            return;
        }

        bool started = policeRaidEvent.StartEventForced();

        if (started)
        {
            Log("Police Raid event started.");
        }
        else
        {
            Log("Police Raid event did not start.");
        }
    }

    public void ForceStartGopnikEvent()
    {
        if (IsAnyEventRunning())
        {
            Log("Cannot force Gopnik event. Another event is already running.");
            return;
        }

        StartGopnikEvent();
    }

    public void ForceStartPoliceRaidEvent()
    {
        if (IsAnyEventRunning())
        {
            Log("Cannot force Police Raid event. Another event is already running.");
            return;
        }

        StartPoliceRaidEvent();
    }

    private bool IsAnyEventRunning()
    {
        bool gopnikRunning = gopnikEvent != null && gopnikEvent.IsEventRunning;
        bool policeRunning = policeRaidEvent != null && policeRaidEvent.IsEventRunning;

        return gopnikRunning || policeRunning;
    }

    private void UpdateChancePreview()
    {
        float totalWeight = gopnikWeight + policeRaidWeight;

        if (totalWeight <= 0f)
        {
            chancePreview =
                "No event can happen because both weights are 0.\n" +
                "No Event: 100%\n" +
                "Gopnik Event: 0%\n" +
                "Police Raid: 0%";
            return;
        }

        float noEventPercent = (1f - totalEventChance) * 100f;
        float gopnikPercent = totalEventChance * (gopnikWeight / totalWeight) * 100f;
        float policePercent = totalEventChance * (policeRaidWeight / totalWeight) * 100f;

        chancePreview =
            "No Event: " + noEventPercent.ToString("0.##") + "%\n" +
            "Gopnik Event: " + gopnikPercent.ToString("0.##") + "%\n" +
            "Police Raid: " + policePercent.ToString("0.##") + "%";
    }

    private void Log(string message)
    {
        if (showDebugLogs)
        {
            Debug.Log("[DropRandomEventManager] " + message);
        }
    }
}