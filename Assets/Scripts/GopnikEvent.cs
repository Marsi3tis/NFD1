using UnityEngine;
using System.Collections.Generic;
using Unity.Cinemachine;
using System;

public class GopnikEvent : MonoBehaviour
{
    [Serializable]
    public struct GopnikVariant
    {
        public Sprite backgroundSprite;
        public string titleText;
        public Sprite characterSprite;
        [TextArea(3, 10)] public string descriptionText;
        public List<string> dialogueOptions;
        [TextArea(3, 10)] public string gopnikBio;
    }

    [Header("Event Settings")]
    [Range(0f, 1.0f)] public float option1;
    [Range(0f, 1.0f)] public float option2;
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
    public List<Sprite> backgroundSprites;    
    public List<GopnikVariant> eventVariants;    
    public float eventChance = 1.0f;
    private GopnikVariant activeVariant;
    private string activeDialogue;
    bool EventEnding;
    private bool eventRunning;
    public bool IsEventRunning
    {
        get { return eventRunning; }
    }
    void Awake()
    {
        Instance = this;
    }

    public void DropIsPicked()
    {
        TryStartEvent();
    }
    public bool TryStartEvent()
    {
        if(eventRunning)
            return false;
        float roll = UnityEngine.Random.Range(0f, 1.0f);
        if(roll > eventChance)
        {
            return false;
        }
        return StartEventForced();
        
    }
    public bool StartEventForced()
    {
        if(eventRunning)
            return false;
        
        if(TopControl.Instance != null)
        {
            TopControl.Instance.StopTheCar();
        }
        RollRandomRobberSprite();
        StartGopnikEvent();
        eventRunning = true;
        return true;
    }

    private void RollRandomRobberSprite()
    {
    if (eventVariants != null && eventVariants.Count > 0)
        {
            int randomIndex = UnityEngine.Random.Range(0, eventVariants.Count);
            activeVariant = eventVariants[randomIndex];
            // NEW: Pick a random line of text from this specific character's pool
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
            activeVariant = new GopnikVariant { titleText = "ERROR", gopnikBio = "ERROR", characterSprite = null };
            activeDialogue = "EVENT ERROR";
        }
    }

    private void EndGopnikEvent()
    {
        if(EventEnding == true)
        {
            // Reset the state machine flag for future events
            EventEnding = false;
            popUpUI.Show(
                activeVariant.titleText,
                activeVariant.descriptionText,
                activeDialogue,
                activeVariant.characterSprite,
                activeVariant.backgroundSprite,
                activeVariant.gopnikBio,
                "Continue",
                () =>
                {
                    popUpUI.Hide();
                }
            );
        }
    }

    private void StartGopnikEvent()
    {
        popUpUI.Show(
            activeVariant.titleText,
            activeVariant.descriptionText,
            activeDialogue,
            activeVariant.characterSprite,
            activeVariant.backgroundSprite,
            activeVariant.gopnikBio,
            "Pisi i barzda",
            () => { OnOption1(); },
            "Bandai pabegt",
            () => { OnOption2(); },
            "atiduok dropus",
            () => { OnOption3(); }
        );
    }
    void OnOption1()
    {
        float roll = UnityEngine.Random.Range(0f, 1.0f);
        if(roll <= option1)
        {
            ExperienceManager.Instance.AddXP(option1Success);
            EventEnding = true;
            EndGopnikEvent();
        }
        else
        {
            ExperienceManager.Instance.RemoveXP(option1Fail);
            RemoveDropsByPercent(option1FailDropLossPercent);
            EventEnding = true;
            EndGopnikEvent();
        }
    }
    void OnOption2()
    {
        float roll = UnityEngine.Random.Range(0f, 1.0f);
        if(roll <= option2)
        {
            ExperienceManager.Instance.AddXP(option2Success);
            EventEnding = true;
            EndGopnikEvent();
        }
        else
        {
            ExperienceManager.Instance.RemoveXP(option2Fail);
            RemoveDropsByPercent(option2FailDropLossPercent);
            EventEnding = true;    
            EndGopnikEvent();            
        }
    }

    void OnOption3()
    {
        ExperienceManager.Instance.RemoveXP(option3);
        RemoveDropsByPercent(option3DropLossPercent);
        EventEnding = true;
        EndGopnikEvent();
    }
    void RemoveDropsByPercent(float percent)
    {
        if (InventoryManager.Instance == null)
            return;
        InventoryManager.Instance.RemovePercentFromEachItem(percent);
    }
}