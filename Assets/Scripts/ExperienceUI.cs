using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ExperienceUI : MonoBehaviour
{
    public TMP_Text levelText;
    public TMP_Text xpText;
    public Slider   xpProgressBar;

    private void Update()
    {
        if (ExperienceManager.Instance == null) return;

        levelText.text     = "Level " + ExperienceManager.Instance.CurrentLevel;
        xpText.text        = ExperienceManager.Instance.CurrentXP + " / " + ExperienceManager.Instance.GetXPForNextLevel() + " XP";
        xpProgressBar.value = ExperienceManager.Instance.GetLevelProgress();
    }
}
