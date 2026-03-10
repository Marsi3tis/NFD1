using UnityEngine;

public class MenuController : MonoBehaviour
{
    [Header("UI Settings")]
    [SerializeField] private GameObject menuPanel;
    bool isActive = false;
   bool Pausedgame = true;
    void Start()
    {

        if (Pausedgame == true)
        {
            Time.timeScale = 0f;
            Debug.Log("Game is paused");
            
            
        }
        else
        {
            Time.timeScale = 1f;
            Debug.Log("Game is not paused");
            
            
        }
       
        /*if (menuPanel != null)
        {
            menuPanel.SetActive(false);
        }*/
       
       /* if (Time.timeScale == 0f)
        {
            ToggleMenu();
            return; // Exit Start early
        }*/

        Debug.Log("TimeScale: " + Time.timeScale);

        // Always start the game running
        /*if (isActive)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;
        }*/

        // Make sure pause menu is hidden
        /*if (menuPanel != null)
            menuPanel.SetActive(false);*/

        Debug.Log("TimeScale1: " + Time.timeScale);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            
            PauseGame();
            Debug.Log("TimeScale2: " + Time.timeScale);
        }
    }

    void ToggleMenu()
    {
        
        if (menuPanel == null) return;
        
        
        isActive = menuPanel.activeSelf;

        if (isActive)
            ResumeGame();
        else
            PauseGame();

        Debug.Log("TimeScale3: " + Time.timeScale);
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
    public void OnResumeButtonPressed()
    {
        ResumeGame();
        Debug.Log("Resumed Game");
        
    }
    public void OnPlayButtonPressed()
    {
        ResumeGame();
        Debug.Log("Playing game");
    }
}