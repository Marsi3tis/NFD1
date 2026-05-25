using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class PoliceRaidEvent : MonoBehaviour
{
    [Serializable]
    public struct PoliceVariant
    {
        public Sprite backgroundSprite;
        public string titleText;
        public Sprite characterSprite;
        [TextArea(3, 10)] public string descriptionText;
        public List<string> dialogueOptions;
        [TextArea(3, 10)] public string policeBio;
    }

    public static PoliceRaidEvent Instance;

    [Header("UI")]
    public EventPopUpUI popUpUI;

    [Header("Visual Asset Settings")]
    public List<PoliceVariant> eventVariants;

    [Header("Standalone Drop Chance")]
    [Tooltip("Used only if DropIsPicked() or TryStartEvent() is called directly. If you use DropRandomEventManager, this chance is ignored.")]
    [Range(0f, 1f)] public float eventChance = 0.15f;

    [Header("Police Raid Chances")]
    [Range(0f, 1f)] public float documentsReleaseChance = 0.45f;
    [Range(0f, 1f)] public float runSuccessChance = 0.30f;
    [Range(0f, 1f)] public float bribeSuccessChance = 0.55f;
    [Range(0f, 1f)] public float hideDropsSuccessChance = 0.40f;

    [Header("Bribe Settings")]
    public int bribeCost = 150;
    [Tooltip("If false, bribe only rolls chance and does not check score/money.")]
    public bool requireCurrencyForBribe = false;
    [Tooltip("Optional manager that contains methods like CanAfford(int) and SpendScore(int).")]
    public MonoBehaviour currencyManager;
    public string canAffordMethodName = "CanAfford";
    public string spendCurrencyMethodName = "SpendScore";

    [Header("Game Over")]
    public bool reloadCurrentSceneOnFail = true;
    public UnityEvent onRaidFailed;
    public UnityEvent onRaidSuccess;

    private PoliceVariant activeVariant;
    private string activeDialogue;
    private bool eventRunning;
    public bool IsEventRunning => eventRunning;

    private void Awake()
    {
        Instance = this;
    }

    public void DropIsPicked()
    {
        TryStartEvent();
    }

    public bool TryStartEvent()
    {
        if (eventRunning)
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

        eventRunning = true;

        if (TopControl.Instance != null)
            TopControl.Instance.StopTheCar();

        RollRandomPoliceVariant();
        ShowDocumentsStage();
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
                activeDialogue = "Pareigūnas: Dokumentus prašom.";
            }
        }
        else
        {
            activeVariant = new PoliceVariant
            {
                titleText = "POLICIJOS REIDAS",
                descriptionText = "Policija sustabdo tavo automobilį ir paprašo dokumentų.",
                policeBio = "Standartinis reido pareigūnas. Bloga diena, blogas žvilgsnis, blogas laikas turėti dropų.",
                characterSprite = null,
                backgroundSprite = null
            };

            activeDialogue = "Pareigūnas: Dokumentus prašom.";
        }
    }

    private void ShowDocumentsStage()
    {
        popUpUI.Show(
            GetTitle(),
            GetDescription(),
            activeDialogue + "\n\n",
            activeVariant.characterSprite,
            activeVariant.backgroundSprite,
            GetBio(),
            "Pateiki",
            OnGiveDocuments,
            "Nepateiki",
            ShowRefuseDocumentsStage
        );
    }

    private void OnGiveDocuments()
    {
        float roll = UnityEngine.Random.Range(0f, 1f);

        if (roll <= documentsReleaseChance)
        {
            EndPoliceEventSuccess(
                "Pareigūnas pavarto dokumentus, pažiūri į tave...\n\n" +
                "-Gerai, laisvas. Gero kelio"
            );
        }
        else
        {
            ShowSearchStage(
                "Pareigūnas per ilgai žiūri į tavo dokumentus.\n\n" +
                "-Būsite apieškomas dėl įtarimo, kad užsiėmėte neteisėta veikla."
            );
        }
    }

    private void ShowRefuseDocumentsStage()
    {
        popUpUI.Show(
            GetTitle(),
            GetDescription(),
            "Atsisakai duoti dokumentus\n\n" +
            "-Dar kartą sakau, dokumentus prašom.",
            activeVariant.characterSprite,
            activeVariant.backgroundSprite,
            GetBio(),
            "Bėgi",
            OnRun,
            "Papirki",
            OnBribeFromRefuseStage
        );
    }

    private void ShowSearchStage(string searchDialogue)
    {
        popUpUI.Show(
            GetTitle(),
            GetDescription(),
            searchDialogue + "\n\nPareigūnas ruošiasi jus apieškoti.",
            activeVariant.characterSprite,
            activeVariant.backgroundSprite,
            GetBio(),
            "Slepi dropus",
            OnHideDrops,
            "Papirki",
            OnBribeFromSearchStage,
            "Bėgi",
            OnRun
        );
    }

    private void OnRun()
    {
        float roll = UnityEngine.Random.Range(0f, 1f);

        if (roll <= runSuccessChance)
        {
            EndPoliceEventSuccess(
                "Rimtas kentas, pabėgai nuo mentų\n\n" +
                "Pareigūnai nespėjo sureaguoti, sėkmingai pabėgai nuo jų."
            );
        }
        else
        {
            EndPoliceEventFail(
                "Buvai pagautas ir supakuotas\n\n" +
                "Istorijos pabaiga. Dabar kilnosi muilą kalėjime"
            );
        }
    }

    private void OnBribeFromRefuseStage()
    {
        TryBribe(
            "Bandai paduoti kyšį vietoj dokumentų.",
            "Pareigūnas pažiūri į kupiūrą, bet jos neima",
            "-Už bandymą papirkti pareigūną, esate sulaikytas."
        );
    }

    private void OnBribeFromSearchStage()
    {
        TryBribe(
            "Kol pareigūnas ruošiasi jus apieškoti, bandai išspręsti situaciją pinigais.",
            "Pareigūnas patenkintas pinigais, palieka tave ramybėje.\n\n",
            "Pareigūnas nepriima kyšio, o už bandymą papirkti pareigūną, esate sulaikytas."
        );
    }

    private void TryBribe(string attemptText, string successText, string failText)
    {
        if (!CanPayBribe())
        {
            ShowSearchStage(
                attemptText + "\n\nKišenėje per mažai pinigų. Pareigūnas neatrodo sužavėtas."
            );
            return;
        }

        SpendBribe();

        float roll = UnityEngine.Random.Range(0f, 1f);
        if (roll <= bribeSuccessChance)
        {
            EndPoliceEventSuccess(successText);
        }
        else
        {
            EndPoliceEventFail(failText);
        }
    }

    private void OnHideDrops()
    {
        float roll = UnityEngine.Random.Range(0f, 1f);

        if (roll <= hideDropsSuccessChance)
        {
            EndPoliceEventSuccess(
                "Susikiši dropus, ten kur saulė nešviečia\n\n" +
                "Pareigūnas apieško mašiną, bet randa tik cigarečių pakelį."
            );
        }
        else
        {
            EndPoliceEventFail(
                "Bandai slėpti dropus, bet pareigūnas viską pamato.\n\n" +
                "Iš karto esate sulaikytas už įtarimą dėl narkotikų platinimo."
            );
        }
    }

    private void EndPoliceEventSuccess(string dialogue)
    {
        popUpUI.Show(
            GetTitle(),
            GetDescription(),
            dialogue,
            activeVariant.characterSprite,
            activeVariant.backgroundSprite,
            GetBio(),
            "Continue",
            () =>
            {
                eventRunning = false;
                onRaidSuccess.Invoke();
                FinishGopnikEvent();
            }
        );
    }

    private void EndPoliceEventFail(string dialogue)
    {
        popUpUI.Show(
            "SULAIKYTAS",
            "Policijos reidas nepavyko.",
            dialogue,
            activeVariant.characterSprite,
            activeVariant.backgroundSprite,
            GetBio(),
            reloadCurrentSceneOnFail ? "Restart" : "Game Over",
            () =>
            {
                eventRunning = false;
                onRaidFailed.Invoke();

                if (reloadCurrentSceneOnFail)
                {
                    Time.timeScale = 1f;
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                }
            }
        );
    }

    private bool CanPayBribe()
    {
        if (!requireCurrencyForBribe)
            return true;

        if (currencyManager == null)
        {
            Debug.LogWarning("PoliceRaidEvent: requireCurrencyForBribe is enabled, but currencyManager is not assigned.");
            return false;
        }

        object result = InvokeCurrencyMethod(canAffordMethodName, bribeCost);
        if (result is bool)
            return (bool)result;

        Debug.LogWarning("PoliceRaidEvent: CanAfford method not found or did not return bool. Method name: " + canAffordMethodName);
        return false;
    }

    private void SpendBribe()
    {
        if (!requireCurrencyForBribe)
            return;

        if (currencyManager == null)
            return;

        object result = InvokeCurrencyMethod(spendCurrencyMethodName, bribeCost);
        if (result == null)
        {
            Debug.LogWarning("PoliceRaidEvent: Spend method not found. Bribe succeeded/failed, but currency was not removed. Method name: " + spendCurrencyMethodName);
        }
    }

    private object InvokeCurrencyMethod(string methodName, int amount)
    {
        Type type = currencyManager.GetType();

        MethodInfo method = type.GetMethod(methodName, new Type[] { typeof(int) });
        if (method != null)
            return method.Invoke(currencyManager, new object[] { amount });

        method = type.GetMethod(methodName, new Type[] { typeof(float) });
        if (method != null)
            return method.Invoke(currencyManager, new object[] { (float)amount });

        method = type.GetMethod(methodName, Type.EmptyTypes);
        if (method != null)
            return method.Invoke(currencyManager, null);

        return null;
    }

    private string GetTitle()
    {
        return string.IsNullOrEmpty(activeVariant.titleText) ? "POLICIJOS REIDAS" : activeVariant.titleText;
    }

    private string GetDescription()
    {
        return string.IsNullOrEmpty(activeVariant.descriptionText)
            ? "Policija sustabdo tavo automobilį. Vienas neteisingas pasirinkimas gali baigti žaidimą."
            : activeVariant.descriptionText;
    }

    private string GetBio()
    {
        return string.IsNullOrEmpty(activeVariant.policeBio)
            ? "Policijos reidas. Sėkmė priklauso nuo pasirinkimo, rizikos ir šiek tiek nuo likimo."
            : activeVariant.policeBio;
    }
    private void FinishGopnikEvent()
    {
        eventRunning = false;
        popUpUI.Hide();
    }
}
