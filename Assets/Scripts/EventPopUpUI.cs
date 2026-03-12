using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EventPopUpUI : MonoBehaviour
{
    GopnikEvent gopnikEvent;
    public GameObject root;
    public TMP_Text EventTitleText;
    public TMP_Text EventDisc;
    public Image GopnikImage;
    public Image Background;

    public Button button1;
    public Button button2;
    public Button button3;

    public TMP_Text button1text;
    public TMP_Text button2text;
    public TMP_Text button3text;
    void Awake()
    {
        gopnikEvent = GetComponent<GopnikEvent>();
    }
    public void Show(
        string title,
        string discription,
        Image image,
        Image background,
        string choice1Text = null, Action choice1Action = null,
        string choice2Text = null, Action choice2Action = null,
        string choice3Text = null, Action choice3Action = null

    )
    {
        root.SetActive(true);
        EventTitleText.text = title;
        EventDisc.text = discription;
        GopnikImage = image;
        Background = background;
        SetupButton(button1, button1text, choice1Text, choice1Action);
        SetupButton(button2, button2text, choice2Text, choice2Action);
        SetupButton(button3, button3text, choice3Text, choice3Action);
        Time.timeScale = 0f;
    }

    private void SetupButton(Button button, TMP_Text textLabel, string text, Action action)
    {
        button.onClick.RemoveAllListeners();
        textLabel.text = text;
        button.onClick.AddListener(() =>
        {
           action?.Invoke(); 
        });
    }
    public void Hide()
    {
        root.SetActive(false);
        Time.timeScale = 1f;
    }
}
