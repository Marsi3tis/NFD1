using UnityEngine;
using System.Collections.Generic;
using Unity.Cinemachine;
using System;

public class GopnikEvent : MonoBehaviour
{
    [Serializable]
    public struct GopnikVariant
    {
        public string titleText;
        public Sprite characterSprite;

        [TextArea(3, 10)]
        public string descriptionText;

        public List<string> dialogueOptions;

        [TextArea(3, 10)]
        public string gopnikBio;
    }

    [Header("Event Settings")]
    [Range(0f, 1.0f)] public float option1; // Pisi i barzda base chance
    [Range(0f, 1.0f)] public float option2; // Pisi zaiba base chance

    public int option1Success = 50;
    public int option1Fail = 70;

    public int option2Success = 20;
    public int option2Fail = 50;

    public int option3 = 100;

    [Header("Drop Loss Settings")]
    [Range(0f, 100f)] public float option1FailDropLossPercent = 30f;
    [Range(0f, 100f)] public float option2FailDropLossPercent = 50f;
    [Range(0f, 100f)] public float option3DropLossPercent = 100f;

    public static GopnikEvent Instance;

    public CinemachineCamera cmCamera;
    public EventPopUpUI popUpUI;

    [Header("Visual Asset Settings")]
    public Sprite backgroundSprite;
    public List<GopnikVariant> eventVariants;

    public float eventChance = 1.0f;

    private GopnikVariant activeVariant;
    private string activeDialogue;
    private bool EventEnding;

    private void Awake()
    {
        Instance = this;
    }

    public void DropIsPicked()
    {
        float roll = UnityEngine.Random.Range(0f, 1.0f);

        if (roll <= eventChance)
        {
            TopControl.Instance.StopTheCar();
            RollRandomRobberSprite();
            StartGopnikEvent();
        }
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
                activeDialogue = "ERROR";
            }
        }
        else
        {
            activeVariant = new GopnikVariant
            {
                titleText = "ERROR",
                gopnikBio = "ERROR",
                characterSprite = null,
                descriptionText = "EVENT ERROR",
                dialogueOptions = null
            };

            activeDialogue = "EVENT ERROR";
        }
    }

    private void StartGopnikEvent()
    {
        string thirdOptionText = "atiduok dropus";

        if (PlayerSkills.Instance != null && PlayerSkills.Instance.CanUseVierhatura)
            thirdOptionText = "Vierhatura";

        popUpUI.Show(
            activeVariant.titleText,
            activeVariant.descriptionText,
            activeDialogue,
            activeVariant.characterSprite,
            backgroundSprite,
            activeVariant.gopnikBio,

            "Pisi i barzda",
            () => { OnOption1(); },

            "Pisi zaiba",
            () => { OnOption2(); },

            thirdOptionText,
            () => { OnOption3(); }
        );
    }

    private void OnOption1()
    {
        float skillBonus = 0f;

        if (PlayerSkills.Instance != null)
            skillBonus = PlayerSkills.Instance.GetMahalkesBonus();

        float finalChance = Mathf.Clamp01(option1 + skillBonus);
        float roll = UnityEngine.Random.Range(0f, 1.0f);

        if (roll <= finalChance)
        {
            ExperienceManager.Instance.AddXP(option1Success);
        }
        else
        {
            ExperienceManager.Instance.RemoveXP(option1Fail);
            RemoveDropsByPercent(option1FailDropLossPercent);
        }

        EventEnding = true;
        EndGopnikEvent();
    }

    private void OnOption2()
    {
        float skillBonus = 0f;

        if (PlayerSkills.Instance != null)
            skillBonus = PlayerSkills.Instance.GetPisiZaibaBonus();

        float finalChance = Mathf.Clamp01(option2 + skillBonus);
        float roll = UnityEngine.Random.Range(0f, 1.0f);

        if (roll <= finalChance)
        {
            ExperienceManager.Instance.AddXP(option2Success);
        }
        else
        {
            ExperienceManager.Instance.RemoveXP(option2Fail);
            RemoveDropsByPercent(option2FailDropLossPercent);
        }

        EventEnding = true;
        EndGopnikEvent();
    }

    private void OnOption3()
    {
        if (PlayerSkills.Instance != null && PlayerSkills.Instance.CanUseVierhatura)
        {
            ExperienceManager.Instance.AddXP(25);
        }
        else
        {
            ExperienceManager.Instance.RemoveXP(option3);
            RemoveDropsByPercent(option3DropLossPercent);
        }

        EventEnding = true;
        EndGopnikEvent();
    }

    private void EndGopnikEvent()
    {
        if (EventEnding == true)
        {
            EventEnding = false;

            popUpUI.Show(
                activeVariant.titleText,
                activeVariant.descriptionText,
                activeDialogue,
                activeVariant.characterSprite,
                backgroundSprite,
                activeVariant.gopnikBio,

                "Continue",
                () =>
                {
                    popUpUI.Hide();
                }
            );
        }
    }

    private void RemoveDropsByPercent(float percent)
    {
        if (InventoryManager.Instance == null)
            return;

        InventoryManager.Instance.RemovePercentFromEachItem(percent);
    }
}