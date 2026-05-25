using UnityEngine;
using TMPro;
using System;
using System.Collections.Generic;
using UnityEngine.Events;


public class PoliceRaidEvent : MonoBehaviour
{
    [Serializable]
    public struct PoliceVariant
    {
        public Sprite backgroundSprite;
        public string titleText;
        public Sprite characterSprite;

        [TextArea(3, 10)]
        public string descriptionText;

        public List<string> dialogueOptions;

        [TextArea(3, 10)]
        public string policeBio;
    }

    [Header("Event Chance")]
    [Range(0f, 1f)] public float eventChance = 0.25f;

    [Header("Option Chances")]
    [Range(0f, 1f)] public float runChance = 0.45f;
    [Range(0f, 1f)] public float hideChance = 0.55f;
    [Range(0f, 1f)] public float riskChance = 0.35f;

    [Header("Money Settings")]
    [Min(1f)] public float bribeCost = 150f;

    [Header("XP Settings")]
    public int runSuccessXP = 30;
    public int runFailXP = 50;
    public int hideSuccessXP = 25;
    public int hideFailXP = 40;
    public int bribeSuccessXP = 20;
    public int bribeFailXP = 60;
    public int riskSuccessXP = 60;
    public int riskFailXP = 80;

    [Header("References")]
    public EventPopUpUI popUpUI;

    [Header("Game Over UI")]
    [SerializeField] private GameObject mainMenuObject;
    [SerializeField] private GameObject inGameObject;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private TMP_Text gameOverReasonText;
    [SerializeField] private bool pauseGameOnGameOver = true;

    [Header("Game Over Event")]
    public UnityEvent onPlayerArrest;

    [Header("Visual Asset Settings")]
    public List<PoliceVariant> eventVariants;

    public static PoliceRaidEvent Instance;

    private PoliceVariant activeVariant;
    private string activeDialogue;
    private bool eventRunning;
    private string lastGameOverReason;

    public bool IsEventRunning => eventRunning;

    private void Awake()
    {
        Instance = this;

        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);
    }

    public void DropIsPicked()
    {
        TryStartEvent();
    }

    public bool TryStartEvent()
    {
        if (eventRunning)
            return false;

        if (GopnikEvent.Instance != null && GopnikEvent.Instance.IsEventRunning)
            return false;

        float roll = UnityEngine.Random.Range(0f, 1f);

        if (roll > eventChance)
            return false;

        return StartEventForced();
    }

    public bool StartEventForced()
    {
        if (eventRunning)
            return false;

        if (GopnikEvent.Instance != null && GopnikEvent.Instance.IsEventRunning)
            return false;

        if (popUpUI == null)
        {
            Debug.LogError("PoliceEvent: popUpUI is not assigned.");
            return false;
        }

        eventRunning = true;

        if (TopControl.Instance != null)
            TopControl.Instance.StopTheCar();

        RollRandomPoliceVariant();
        StartPoliceEvent();

        return true;
    }

    private void RollRandomPoliceVariant()
    {
        if (eventVariants != null && eventVariants.Count > 0)
        {
            int randomIndex = UnityEngine.Random.Range(0, eventVariants.Count);
            activeVariant = eventVariants[randomIndex];

            if (activeVariant.dialogueOptions != null && activeVariant.dialogueOptions.Count > 0)
            {
                int randomTextIndex = UnityEngine.Random.Range(0, activeVariant.dialogueOptions.Count);
                activeDialogue = activeVariant.dialogueOptions[randomTextIndex];
            }
            else
            {
                activeDialogue = "Pareigunas prieina prie lango ir laukia tavo sprendimo.";
            }
        }
        else
        {
            activeVariant = new PoliceVariant
            {
                backgroundSprite = null,
                characterSprite = null,
                titleText = "Police event error",
                descriptionText = "Nera priskirtu police event variantu.",
                dialogueOptions = null,
                policeBio = "Patikrink PoliceEvent Inspector lange."
            };

            activeDialogue = "Truksta eventVariants.";
        }
    }

    private void StartPoliceEvent()
    {
        popUpUI.Show(
            SafeText(activeVariant.titleText, "Policijos reidas"),
            SafeText(activeVariant.descriptionText, "Tave sustabde policija."),
            SafeText(activeDialogue, "Ka darai?"),
            activeVariant.characterSprite,
            activeVariant.backgroundSprite,
            SafeText(activeVariant.policeBio, ""),
            "Begti",
            OnRun,
            "Slepti dropus",
            OnHideDrops,
            "Papirkti",
            OnBribe
        );
    }

    private void OnRun()
    {
        float roll = UnityEngine.Random.Range(0f, 1f);

        if (roll <= runChance)
        {
            AddXP(runSuccessXP);
            ShowSuccessResult("Pavyko pabegti. Pareigunai liko su svytureliais toli uz nugaros.");
        }
        else
        {
            FailPoliceEvent(
                runFailXP,
                "Nepavyko pabegti. Pareigunai tave pagauna.",
                "Game Over: tave sueme policija po nepavykusio begimo."
            );
        }
    }

    private void OnHideDrops()
    {
        float roll = UnityEngine.Random.Range(0f, 1f);

        if (roll <= hideChance)
        {
            AddXP(hideSuccessXP);
            ShowSuccessResult("Dropus paslepei laiku. Pareigunas nieko nerado.");
        }
        else
        {
            FailPoliceEvent(
                hideFailXP,
                "Paslepimas nepavyko. Pareigunas rado dropus.",
                "Game Over: policija rado dropus ir tave sueme."
            );
        }
    }

    private void OnBribe()
    {
        if (bribeCost <= 0f)
            bribeCost = 150f;

        if (MoneyManager.Instance == null)
        {
            FailPoliceEvent(
                bribeFailXP,
                "Bandai papirkti pareiguna, bet pinigu sistema neveikia. Pareigunas nusprendzia, kad gana cirko.",
                "Game Over: tave sueme uz bandyma papirkti pareiguna."
            );
            return;
        }

        if (!MoneyManager.Instance.CanAfford(bribeCost))
        {
            FailPoliceEvent(
                bribeFailXP,
                $"Bandai duoti kysi, bet neturi {bribeCost} cash. Pareigunas supranta, ka bandai padaryti.",
                "Game Over: tave sueme uz bandyma papirkti pareiguna."
            );
            return;
        }

        bool paid = MoneyManager.Instance.SpendMoney(bribeCost);

        if (!paid)
        {
            FailPoliceEvent(
                bribeFailXP,
                "Kysis nepavyko. Pareigunas supranta, ka bandai padaryti.",
                "Game Over: tave sueme uz bandyma papirkti pareiguna."
            );
            return;
        }

        AddXP(bribeSuccessXP);
        ShowSuccessResult($"Pareigunas paeme {bribeCost} cash ir nusprende, kad siandien nieko nemate.");
    }

    public void OnRisk()
    {
        float roll = UnityEngine.Random.Range(0f, 1f);

        if (roll <= riskChance)
        {
            AddXP(riskSuccessXP);
            ShowSuccessResult("Kazkaip praslydai. Net pareigunas liko sutrikes.");
        }
        else
        {
            FailPoliceEvent(
                riskFailXP,
                "Rizika nepasiteisino. Pareigunai padare pilna patikra.",
                "Game Over: policija tave sueme po nesekmingos patikros."
            );
        }
    }

    private void ShowSuccessResult(string resultText)
    {
        popUpUI.Show(
            SafeText(activeVariant.titleText, "Policijos reidas"),
            SafeText(activeVariant.descriptionText, ""),
            resultText,
            activeVariant.characterSprite,
            activeVariant.backgroundSprite,
            SafeText(activeVariant.policeBio, ""),
            "Continue",
            FinishPoliceEvent
        );
    }

    private void FailPoliceEvent(int xpLoss, string resultText, string gameOverReason)
    {
        RemoveXP(xpLoss);
        RemoveDropsByPercent(100f);

        lastGameOverReason = gameOverReason;

        popUpUI.Show(
            SafeText(activeVariant.titleText, "Policijos reidas"),
            SafeText(activeVariant.descriptionText, ""),
            resultText,
            activeVariant.characterSprite,
            activeVariant.backgroundSprite,
            SafeText(activeVariant.policeBio, ""),
            "Continue",
            FinishPoliceEventWithGameOver
        );
    }

    private void FinishPoliceEvent()
    {
        eventRunning = false;

        if (popUpUI != null)
            popUpUI.Hide();
    }

    private void FinishPoliceEventWithGameOver()
    {
        eventRunning = false;

        if (popUpUI != null)
            popUpUI.Hide();

        EndGame();
    }

    private void EndGame()
    {
        onPlayerArrest?.Invoke();
        if (inGameObject != null)
            inGameObject.SetActive(false);
        
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);

            if (gameOverReasonText != null)
                gameOverReasonText.text = SafeText(lastGameOverReason, "Game Over");

            if (pauseGameOnGameOver)
                Time.timeScale = 0f;

            return;
        }
        ShowMainMenu();
    }
    public void ShowMainMenu()
    {
        if (popUpUI != null)
            popUpUI.Hide();

        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

        if (inGameObject != null)
            inGameObject.SetActive(false);

        if (mainMenuObject != null)
            mainMenuObject.SetActive(true);
        else
            Debug.LogError("PoliceEvent: Main Menu Object is not assigned.");

        if (pauseGameOnGameOver)
            Time.timeScale = 0f;
    }

    public void RestartAfterGameOver()
    {
        Time.timeScale = 1f;

        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

        if (inGameObject != null)
            inGameObject.SetActive(true);

        if (mainMenuObject != null)
            mainMenuObject.SetActive(true);
    }
    private void AddXP(int amount)
    {
        if (ExperienceManager.Instance != null)
            ExperienceManager.Instance.AddXP(amount);
    }

    private void RemoveXP(int amount)
    {
        if (ExperienceManager.Instance != null)
            ExperienceManager.Instance.RemoveXP(amount);
    }

    private void RemoveDropsByPercent(float percent)
    {
        if (InventoryManager.Instance != null)
            InventoryManager.Instance.RemovePercentFromEachItem(percent);
    }

    private string SafeText(string value, string fallback)
    {
        if (string.IsNullOrWhiteSpace(value))
            return fallback;

        return value;
    }
}