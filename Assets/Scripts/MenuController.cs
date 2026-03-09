using UnityEngine;

public class MenuController : MonoBehaviour
{
    [Header("UI Settings")]
    [SerializeField] private GameObject menuPanel;

    void Start()
    {
        Debug.Log("TimeScale: " + Time.timeScale);
        // Always start the game running
        Time.timeScale = 1f;

        // Make sure pause menu is hidden
        if (menuPanel != null)
            menuPanel.SetActive(false);
        Debug.Log("TimeScale: " + Time.timeScale);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleMenu();
            Debug.Log("TimeScale: " + Time.timeScale);
        }
    }

    void ToggleMenu()
    {
        if (menuPanel == null) return;

        bool isActive = menuPanel.activeSelf;

        if (isActive)
            ResumeGame();
        else
            PauseGame();
        Debug.Log("TimeScale: " + Time.timeScale);
    }

    void PauseGame()
    {
        Time.timeScale = 0f;
        menuPanel.SetActive(true);

    }

    void ResumeGame()
    {
        Time.timeScale = 1f;
        menuPanel.SetActive(false);
    }
}