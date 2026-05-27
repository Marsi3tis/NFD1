using UnityEngine;
using System;
using System.Collections.Generic;

public class GopnikEvent : MonoBehaviour
{
    [Serializable]
    public struct GopnikVariant
    {
        public Sprite backgroundSprite;
        public string titleText;
        public Sprite characterSprite;

        [TextArea(3, 10)]
        public string descriptionText;

        public List<string> dialogueOptions;

        [TextArea(3, 10)]
        public string gopnikBio;
    }

    [Header("Event Settings")]
    [Range(0f, 1f)] public float option1 = 0.5f;
    [Range(0f, 1f)] public float option2 = 0.5f;

    public int option1Success = 50;
    public int option1Fail = 70;
    public int option2Success = 20;
    public int option2Fail = 50;
    public int option3 = 100;

    [Header("Drop Loss Settings")]
    [Range(0f, 100f)] public float option1FailDropLossPercent = 30f;
    [Range(0f, 100f)] public float option2FailDropLossPercent = 50f;
    [Range(0f, 100f)] public float option3DropLossPercent = 100f;

    [Header("References")]
    public EventPopUpUI popUpUI;

    [Header("Visual Asset Settings")]
    public List<GopnikVariant> eventVariants;

    [Range(0f, 1f)]
    public float eventChance = 1f;

    public static GopnikEvent Instance;

    private GopnikVariant activeVariant;
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
        
        if (PoliceRaidEvent.Instance != null && PoliceRaidEvent.Instance.IsEventRunning)
            return false;

        float roll = UnityEngine.Random.Range(0f, 1f);

        if (roll > eventChance)
            return false;

        return StartEventForced();
    }

    public bool StartEventForced()
    {
        if (PoliceRaidEvent.Instance != null && PoliceRaidEvent.Instance.IsEventRunning)
            return false;

        if (eventRunning)
            return false;

        if (popUpUI == null)
        {
            Debug.LogError("GopnikEvent: popUpUI is not assigned.");
            return false;
        }

        eventRunning = true;

        if (TopControl.Instance != null)
            TopControl.Instance.StopTheCar();

        RollRandomRobberSprite();
        StartGopnikEvent();

        return true;
    }

    private void RollRandomRobberSprite()
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
                activeDialogue = "Marozas ziuri i tave ir laukia sprendimo.";
            }
        }
        else
        {
            activeVariant = new GopnikVariant
            {
                backgroundSprite = null,
                characterSprite = null,
                titleText = "Event error",
                descriptionText = "Nera priskirtu event variantu.",
                dialogueOptions = null,
                gopnikBio = "Patikrink GopnikEvent Inspector lange."
            };

            activeDialogue = "Truksta eventVariants.";
        }
    }

    private void StartGopnikEvent()
    {
        string thirdOptionText = "Atiduok dropus";

        if (PlayerSkills.Instance != null && PlayerSkills.Instance.CanUseVierhatura)
            thirdOptionText = "Vierhatura";

        popUpUI.Show(
            SafeText(activeVariant.titleText, "Marozu uzpuolimas"),
            SafeText(activeVariant.descriptionText, "Tave uzspaude vietiniai marozai."),
            SafeText(activeDialogue, "Ka darai?"),
            activeVariant.characterSprite,
            activeVariant.backgroundSprite,
            SafeText(activeVariant.gopnikBio, ""),
            "Pisi i barzda",
            OnOption1,
            "Pisi zaiba",
            OnOption2,
            thirdOptionText,
            OnOption3
        );
    }

    private void OnOption1()
    {
        float skillBonus = 0f;

        if (PlayerSkills.Instance != null)
            skillBonus = PlayerSkills.Instance.GetMahalkesBonus();

        float finalChance = Mathf.Clamp01(option1 + skillBonus);
        float roll = UnityEngine.Random.Range(0f, 1f);

        if (roll <= finalChance)
        {
            AddXP(option1Success);
            ShowResult("Pavyko. Marozas nesuprato kas ivyko ir atsitrauke.");
        }
        else
        {
            RemoveXP(option1Fail);
            RemoveDropsByPercent(option1FailDropLossPercent);
            ShowResult("Nepavyko. Gavai atgal ir praradai dali dropu.");
        }
    }

    private void OnOption2()
    {
        float skillBonus = 0f;

        if (PlayerSkills.Instance != null)
            skillBonus = PlayerSkills.Instance.GetPisiZaibaBonus();

        float finalChance = Mathf.Clamp01(option2 + skillBonus);
        float roll = UnityEngine.Random.Range(0f, 1f);

        if (roll <= finalChance)
        {
            AddXP(option2Success);
            ShowResult("Pavyko pabegti. Marozai liko kazkur uz nugaros.");
        }
        else
        {
            RemoveXP(option2Fail);
            RemoveDropsByPercent(option2FailDropLossPercent);
            ShowResult("Nepavyko pabegti. Marozai pasivijo ir nupurte dali dropu.");
        }
    }

    private void OnOption3()
    {
        if (PlayerSkills.Instance != null && PlayerSkills.Instance.CanUseVierhatura)
        {
            AddXP(25);
            ShowResult("Vierhatura suveike. Marozai suprato, kad geriau nesikabineti.");
        }
        else
        {
            RemoveXP(option3);
            RemoveDropsByPercent(option3DropLossPercent);
            ShowResult("Atidavei dropus. Marozai patenkinti, tu nelabai.");
        }
    }

    private void ShowResult(string resultText)
    {
        popUpUI.Show(
            SafeText(activeVariant.titleText, "Marozu uzpuolimas"),
            SafeText(activeVariant.descriptionText, ""),
            resultText,
            activeVariant.characterSprite,
            activeVariant.backgroundSprite,
            SafeText(activeVariant.gopnikBio, ""),
            "Continue",
            FinishGopnikEvent
        );
    }

    private void FinishGopnikEvent()
    {
        eventRunning = false;

        if (popUpUI != null)
            popUpUI.Hide();
    }

    private void AddXP(int amount)
    {
        if (ExperienceManager.Instance != null)
            ExperienceManager.Instance.AddXP(amount);
        if (GameOverManager.Instance != null)
        {
            GameOverManager.Instance.RegisterGopnikEscape();
        }
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
    public void ResetEventState()
    {
        eventRunning = false;
        if (popUpUI != null)
            popUpUI.Hide();
    }
}