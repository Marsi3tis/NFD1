using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EventPopUpUI : MonoBehaviour
{
    public static bool IsEventActive { get; private set; }

    private GopnikEvent gopnikEvent;

    public GameObject root;

    public TMP_Text EventTitleText;
    public TMP_Text EventDisc;
    public TMP_Text EventDialogueText;

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

    private void Awake()
    {
        gopnikEvent = GetComponent<GopnikEvent>();

        if (infoButton != null)
            infoButton.onClick.AddListener(ToggleInfoPanel);

        IsEventActive = false;
    }

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
        IsEventActive = true;
        CarSFX carSFX = FindFirstObjectByType<CarSFX>();
        if (carSFX != null)
            carSFX.StopTireSlideImmediately();

        if (root != null)
            root.SetActive(true);

        if (infoPanel != null)
            infoPanel.SetActive(false);

        if (EventTitleText != null)
            EventTitleText.text = title;

        if (EventDisc != null)
            EventDisc.text = description;

        if (EventDialogueText != null)
            EventDialogueText.text = dialogue;

        currentGopnikBio = gopnikBio;

        if (GopnikImage != null)
        {
            if (characterSprite != null)
            {
                GopnikImage.sprite = characterSprite;
                GopnikImage.gameObject.SetActive(true);
            }
            else
            {
                GopnikImage.gameObject.SetActive(false);
            }
        }

        if (Background != null && backgroundSprite != null)
            Background.sprite = backgroundSprite;

        SetupButton(button1, button1text, choice1Text, choice1Action);
        SetupButton(button2, button2text, choice2Text, choice2Action);
        SetupButton(button3, button3text, choice3Text, choice3Action);

        Time.timeScale = 0f;
    }

    public void Hide()
    {
        IsEventActive = false;

        if (infoPanel != null)
            infoPanel.SetActive(false);

        if (root != null)
            root.SetActive(false);

        Time.timeScale = 1f;
    }

    private void ToggleInfoPanel()
    {
        if (infoPanel == null || infoPanelText == null)
            return;

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
        if (button == null)
            return;

        button.onClick.RemoveAllListeners();

        if (string.IsNullOrEmpty(text))
        {
            button.gameObject.SetActive(false);
            return;
        }

        button.gameObject.SetActive(true);

        if (textLabel != null)
            textLabel.text = text;

        button.onClick.AddListener(() =>
        {
            action?.Invoke();
        });
    }
}