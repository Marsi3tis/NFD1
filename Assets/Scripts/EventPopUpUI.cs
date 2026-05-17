using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EventPopUpUI : MonoBehaviour
{
    GopnikEvent gopnikEvent;
    public GameObject root; // GameObject that contains the entire pop-up UI, to enable/disable as needed
    public TMP_Text EventTitleText; // TextMesh
    public TMP_Text EventDisc; // TextMesh
    public TMP_Text EventDialogueText; // TextMesh 
    
    // These remain UI Image components attached to your Canvas UI GameObjects
    public Image GopnikImage; 
    public Image Background;

    public Button button1;
    public Button button2;
    public Button button3;

    public TMP_Text button1text;
    public TMP_Text button2text;
    public TMP_Text button3text;

    [Header("INFO BUTTON")]
    public Button infoButton;
    public GameObject infoPanel;
    public TMP_Text infoPanelText;
    private string currentGopnikBio;

    void Awake()
    {
        gopnikEvent = GetComponent<GopnikEvent>();
        if (infoButton != null)
        {
            infoButton.onClick.AddListener(ToggleInfoPanel);
        }
    }

    // CHANGED: Now accepts Sprite data rather than UI component references
    public void Show(
        string title,
        string description,
        string dialogue,
        Sprite characterSprite,
        Sprite backgroundSprite,
        string gopnikBio,
        string choice1Text = null, Action choice1Action = null,
        string choice2Text = null, Action choice2Action = null,
        string choice3Text = null, Action choice3Action = null
    )
    {
        root.SetActive(true);
        if(infoPanel != null) infoPanel.SetActive(false);
        EventTitleText.text = title;
        EventDisc.text = description;
        

        if(EventDialogueText != null)
        {
            EventDialogueText.text = dialogue;
        }
        currentGopnikBio = gopnikBio; 
        if (characterSprite != null)
        {
            GopnikImage.sprite = characterSprite;
            GopnikImage.gameObject.SetActive(true);
        }
        else
        {
            GopnikImage.gameObject.SetActive(false);
        }

        if (backgroundSprite != null)
        {
            Background.sprite = backgroundSprite;
        }

        SetupButton(button1, button1text, choice1Text, choice1Action);
        SetupButton(button2, button2text, choice2Text, choice2Action);
        SetupButton(button3, button3text, choice3Text, choice3Action);
        
        Time.timeScale = 0f;
    }
    private void ToggleInfoPanel()
    {
        if (infoPanel == null || infoPanelText == null) return;
        
        if (!infoPanel.activeSelf)
        {
            infoPanelText.text = currentGopnikBio;
            infoPanel.SetActive(true);
        }
        else
        {
            infoPanel.SetActive(false);
        }
    }

    private void SetupButton(Button button, TMP_Text textLabel, string text, Action action)
    {
        button.onClick.RemoveAllListeners();
        
        // Safety check: if no choice text is provided, turn off the button completely
        if (string.IsNullOrEmpty(text))
        {
            button.gameObject.SetActive(false);
            return;
        }

        button.gameObject.SetActive(true);
        textLabel.text = text;
        button.onClick.AddListener(() =>
        {
           action?.Invoke(); 
        });
    }

    public void Hide()
    {
        if(infoPanel != null) infoPanel.SetActive(false);
        root.SetActive(false);
        Time.timeScale = 1f;
    }
}