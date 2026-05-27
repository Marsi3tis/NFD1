using UnityEngine;

public class GameSessionManager : MonoBehaviour
{
    [Header("Drop Reset")]
    [SerializeField] private DropSpawnerPoints dropSpawnerPoints;

    [Header("Screen Objects")]
    [SerializeField] private GameObject mainMenuObject;
    [SerializeField] private GameObject inGameObject;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject eventPopupRoot;

    [Header("Player Reset")]
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform playerStartPoint;
    [SerializeField] private Rigidbody2D playerRigidbody;
    [SerializeField] private TopControl movementScript;

    [Header("Systems")]
    [SerializeField] private FuelSystem fuelSystem;

    public void StartNewGame()
    {
        Time.timeScale = 1f;

        if (mainMenuObject != null)
            mainMenuObject.SetActive(false);

        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

        if (eventPopupRoot != null)
            eventPopupRoot.SetActive(false);

        if (inGameObject != null)
            inGameObject.SetActive(true);

        ResetPlayer();
        ResetManagers();
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f;

        if (eventPopupRoot != null)
            eventPopupRoot.SetActive(false);

        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

        if (inGameObject != null)
            inGameObject.SetActive(false);

        if (mainMenuObject != null)
            mainMenuObject.SetActive(true);
    }

    private void ResetPlayer()
    {
        if (playerTransform != null && playerStartPoint != null)
        {
            playerTransform.position = playerStartPoint.position;
            playerTransform.rotation = playerStartPoint.rotation;
        }

        if (playerRigidbody != null)
        {
            playerRigidbody.linearVelocity = Vector2.zero;
            playerRigidbody.angularVelocity = 0f;
        }

        if (movementScript != null)
            movementScript.enabled = true;
    }

    private void ResetManagers()
    {
        if (fuelSystem != null)
            fuelSystem.ResetFuel();

        if (dropSpawnerPoints != null)
            dropSpawnerPoints.ResetSpawner();
        
        if (MoneyManager.Instance != null)
            MoneyManager.Instance.ResetMoney();

        if (ExperienceManager.Instance != null)
            ExperienceManager.Instance.ResetProgress();

        if (InventoryManager.Instance != null)
            InventoryManager.Instance.ResetInventory();

        if (PlayerSkills.Instance != null)
            PlayerSkills.Instance.ResetSkills();

        if (GopnikEvent.Instance != null)
            GopnikEvent.Instance.ResetEventState();

        if (PoliceRaidEvent.Instance != null)
            PoliceRaidEvent.Instance.ResetEventState();

        if (GameOverManager.Instance != null)
            GameOverManager.Instance.ResetRunStats();
        
    }
}