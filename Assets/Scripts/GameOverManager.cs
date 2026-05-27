using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverManager : MonoBehaviour
{
    public static GameOverManager Instance { get; private set; }
    [Header("Background")]
    [SerializeField] private Image backgroundImage;
    [SerializeField] private Sprite fuelGameOverBackground;
    [SerializeField] private Sprite policeGameOverBackground;

    [Header("Cameras")]
    [SerializeField] private GameObject uiCameraObject;
    [SerializeField] private Camera uiCamera;

    [Header("Screen Objects")]
    [SerializeField] private GameObject mainMenuObject;
    [SerializeField] private GameObject inGameObject;
    [SerializeField] private GameObject gameOverPanel;

    [Header("Game Over Texts")]
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text reasonText;
    [SerializeField] private TMP_Text rankText;
    [SerializeField] private TMP_Text statsText;

    [Header("Buttons")]
    [SerializeField] private Button mainMenuButton;

    private int gopnikEscapes;
    private int policeEscapes;
    private bool gameEnded;

    public bool IsGameEnded => gameEnded;

    private void Awake()
    {
        Instance = this;

        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

        if (mainMenuButton != null)
        {
            mainMenuButton.onClick.RemoveAllListeners();
            mainMenuButton.onClick.AddListener(ReturnToMainMenu);
        }
    }

    public void RegisterGopnikEscape()
    {
        gopnikEscapes++;
    }

    public void RegisterPoliceEscape()
    {
        policeEscapes++;
    }

    public void ShowFuelEmptyGameOver()
    {
        if (backgroundImage != null && fuelGameOverBackground != null)
            backgroundImage.sprite = fuelGameOverBackground;
        ShowGameOver("Baigėsi kuras", "Game Over: pasibaigė kuras.");
    }

    public void ShowPoliceArrestGameOver(string reason)
    {
        if (backgroundImage != null && policeGameOverBackground != null)
            backgroundImage.sprite = policeGameOverBackground;
        ShowGameOver("Suėmė policija", reason);
    }

    private void ShowGameOver(string title, string reason)
    {
        gameEnded = true;

        Time.timeScale = 0f;

        if(uiCameraObject != null)
            uiCameraObject.SetActive(true);
        
        if (uiCamera != null)
            uiCamera.enabled = true;

        if (mainMenuObject != null)
            mainMenuObject.SetActive(false);

        if (inGameObject != null)
            inGameObject.SetActive(false);

        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);

        if (titleText != null)
            titleText.text = title;

        if (reasonText != null)
            reasonText.text = reason;

        string rank = "Nežinomas";

        if (ExperienceManager.Instance != null)
            rank = ExperienceManager.Instance.CurrentRank;

        if (rankText != null)
            rankText.text = "Rankas: " + rank;

        if (statsText != null)
        {
            statsText.text =
                "Išsisukai nuo marozų: " + gopnikEscapes + "\n" +
                "Išsisukai nuo policijos: " + policeEscapes;
        }
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f;
        gameEnded = false;

        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

        if (inGameObject != null)
            inGameObject.SetActive(false);

        if (mainMenuObject != null)
            mainMenuObject.SetActive(true);
    }

    public void ResetRunStats()
    {
        gopnikEscapes = 0;
        policeEscapes = 0;
        gameEnded = false;
    }
}