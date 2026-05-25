using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillMenuUI : MonoBehaviour
{
    [Header("Menu Objects")]
    public GameObject skillMenuPanel;
    public Button openSkillMenuButton;

    [Header("Text Fields")]
    public TMP_Text availableSkillPointsText;
    public TMP_Text mahalkesText;
    public TMP_Text pisiZaibaText;
    public TMP_Text vierhaturaText;

    [Header("Upgrade Buttons")]
    public Button mahalkesButton;
    public Button pisiZaibaButton;
    public Button vierhaturaButton;

    private void Start()
    {
        if (skillMenuPanel != null)
            skillMenuPanel.SetActive(false);

        RefreshUI();
    }

    private void Update()
    {
        RefreshUI();
    }

    public void TryOpenSkillMenu()
    {
        if (ExperienceManager.Instance == null)
            return;

        if (ExperienceManager.Instance.SkillPoints <= 0)
        {
            UnityEngine.Debug.Log("You need at least 1 skill point to open skill menu.");
            return;
        }

        if (skillMenuPanel != null)
            skillMenuPanel.SetActive(true);

        if (openSkillMenuButton != null)
            openSkillMenuButton.gameObject.SetActive(false);

        RefreshUI();
    }

    public void CloseSkillMenu()
    {
        if (skillMenuPanel != null)
            skillMenuPanel.SetActive(false);

        if (openSkillMenuButton != null)
            openSkillMenuButton.gameObject.SetActive(true);
    }

    public void UpgradeMahalkes()
    {
        if (PlayerSkills.Instance == null)
            return;

        PlayerSkills.Instance.UpgradeMahalkes();
        RefreshUI();
        CloseIfNoSkillPoints();
    }

    public void UpgradePisiZaiba()
    {
        if (PlayerSkills.Instance == null)
            return;

        PlayerSkills.Instance.UpgradePisiZaiba();
        RefreshUI();
        CloseIfNoSkillPoints();
    }

    public void UpgradeVierhatura()
    {
        if (PlayerSkills.Instance == null)
            return;

        PlayerSkills.Instance.UpgradeVierhatura();
        RefreshUI();
        CloseIfNoSkillPoints();
    }

    private void CloseIfNoSkillPoints()
    {
        if (ExperienceManager.Instance == null)
            return;

        if (ExperienceManager.Instance.SkillPoints <= 0)
            CloseSkillMenu();
    }

    private void RefreshUI()
    {
        if (ExperienceManager.Instance == null || PlayerSkills.Instance == null)
            return;

        int skillPoints = ExperienceManager.Instance.SkillPoints;

        if (openSkillMenuButton != null)
            openSkillMenuButton.interactable = skillPoints > 0;

        if (availableSkillPointsText != null)
            availableSkillPointsText.text = "Skill Points: " + skillPoints;

        if (mahalkesText != null)
        {
            int percent = Mathf.RoundToInt(PlayerSkills.Instance.GetMahalkesBonus() * 100f);

            mahalkesText.text = "Mahalkes: " +
                                PlayerSkills.Instance.MahalkesLevel +
                                "/" +
                                PlayerSkills.Instance.maxMahalkesLevel +
                                "  +" +
                                percent +
                                "%";
        }

        if (pisiZaibaText != null)
        {
            int percent = Mathf.RoundToInt(PlayerSkills.Instance.GetPisiZaibaBonus() * 100f);

            pisiZaibaText.text = "Pisi zaiba: " +
                                 PlayerSkills.Instance.PisiZaibaLevel +
                                 "/" +
                                 PlayerSkills.Instance.maxPisiZaibaLevel +
                                 "  +" +
                                 percent +
                                 "%";
        }

        if (vierhaturaText != null)
        {
            string status = PlayerSkills.Instance.HasVierhatura ? "Unlocked" : "Locked";
            vierhaturaText.text = "Vierhatura: " + status;
        }

        if (mahalkesButton != null)
        {
            mahalkesButton.interactable =
                skillPoints > 0 &&
                PlayerSkills.Instance.MahalkesLevel < PlayerSkills.Instance.maxMahalkesLevel;
        }

        if (pisiZaibaButton != null)
        {
            pisiZaibaButton.interactable =
                skillPoints > 0 &&
                PlayerSkills.Instance.PisiZaibaLevel < PlayerSkills.Instance.maxPisiZaibaLevel;
        }

        if (vierhaturaButton != null)
        {
            vierhaturaButton.interactable =
                skillPoints > 0 &&
                !PlayerSkills.Instance.HasVierhatura;
        }
    }
}