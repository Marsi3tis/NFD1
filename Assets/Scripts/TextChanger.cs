using UnityEngine;
using TMPro;

public class TextChanger : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI titleText;

    public void ChangeText()
    {
        if (titleText != null)
        {
            titleText.text = "Choose Your Car";
        }
        else
        {
            Debug.LogError("TitleText is not assigned in the Inspector!");
        }
    }
}