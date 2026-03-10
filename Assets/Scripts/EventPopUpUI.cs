using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EventPopUpUI : MonoBehaviour
{
    public GameObject root;
    public TMP_Text EventTitleText;
    public Image GopnikImage;
    public Image Background;

    public Button button1;
    public Button button2;
    public Button button3;

    public TMP_Text button1text;
    public TMP_Text button2text;
    public TMP_Text button3text;
    public void Show(
        string title,
        Image image,
        Image background,
        string choice1Text, Action choice1Action,
        string choice2Text, Action choice2Action,
        string choice3Text, Action choice3Action

    )
    {
        root.SetActive(true);
        EventTitleText.text = title;
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
           Hide(); 
        });
    }
    public void Hide()
    {
        root.SetActive(false);
        Time.timeScale = 1f;
    }
}
