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
                }
                else
                {
                    PauseGame();

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