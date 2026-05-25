using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ExperienceUI : MonoBehaviour
{
    public TMP_Text levelText;
    public TMP_Text rankText;
    public TMP_Text xpText;
    public TMP_Text skillPointsText;
    public Slider xpProgressBar;

    private void Update()
    {
        if (ExperienceManager.Instance == null)
            return;

        if (levelText != null)
            levelText.text = "Rank: " + ExperienceManager.Instance.CurrentRank;

        if (rankText != null)
            rankText.text = "";

        if (xpText != null)
            xpText.text = ExperienceManager.Instance.CurrentXP + " / " +
                          ExperienceManager.Instance.GetXPForNextLevel() + " XP";

        if (skillPointsText != null)
            skillPointsText.text = "Skill Points: " + ExperienceManager.Instance.SkillPoints;

        if (xpProgressBar != null)
            xpProgressBar.value = ExperienceManager.Instance.GetLevelProgress();
    }
}