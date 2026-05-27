using UnityEngine;

public class MenuController : MonoBehaviour
{
    [Header("UI Settings")]
    [SerializeField] private GameObject menuPanel;
    [SerializeField] private GameObject GameManager;

    void Start()
    {

    }

    void Update()
    {
        if (GameManager.activeSelf == true)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (menuPanel.activeSelf)
                {
                    ResumeGame();
                    Debug.Log("Game Resumed");
                }
                else
                {
                    PauseGame();
                    Debug.Log("Game Paused 2");

                }
            }
        }
        else 
        { 
            Time.timeScale = 1f;
            Debug.Log("Nothing Ever Happens"); 
        }
    }


    void PauseGame()
    {
        
        Time.timeScale = 0f;
        menuPanel.SetActive(true);
        Debug.Log("Game Paused 1");
    }

    void ResumeGame()
    {
        
        Time.timeScale = 1f;
        menuPanel.SetActive(false);
    }
    public void OnResumeButtonPressed()
    {
        ResumeGame();
        
        
    }
    public void OnPlayButtonPressed()
    {
        ResumeGame();
        
    }
}