using UnityEngine;
using UnityEngine.UI;
public class GopnikEvent : MonoBehaviour
{
    [Header("Event Settings")]
    [Range(0f, 1.0f)] public float option1;
    [Range(0f, 1.0f)] public float option2;
    public int option1Success = 50;
    public int option1Fail = 70;
    public int option2Success = 20;
    public int option2Fail = 50;
    public int option3 = 100;

    public static GopnikEvent Instance;
    public EventPopUpUI popUpUI;
    public Image backgroundImage;
    public Image robberImage;
    public float eventChance = 1.0f;
    void Awake()
    {
        
        Instance = this;
    }
    public void DropIsPicked()
    {
        float roll = Random.Range(0f, 1.0f);
        if(roll <= eventChance)
        {
            StartGopnikEvent();
            
        }
    }
    private void StartGopnikEvent()
    {
        TopControl.Instance.StopTheCar();
        popUpUI.Show(
            "Title",
            backgroundImage,
            robberImage,
            "Pisi i barzda",
            () =>
            {
                Debug.Log("eventtt");
                OnOption1();
            },
            "Bandai pabegt",
            () =>
            {
                OnOption2();
            },
            "atiduok dropus",
            () =>
            {
                OnOption3();
            }

        );
        void OnOption1()
        {
            float roll = Random.Range(0f, 1.0f);
            if(roll <= option1)
            {
                ExperienceManager.Instance.AddXP(option1Success);
            }
            else
            {
                ExperienceManager.Instance.RemoveXP(option1Fail);
            }
        }
        void OnOption2()
        {
            float roll = Random.Range(0f, 1.0f);
            if(roll <= option1)
            {
                ExperienceManager.Instance.AddXP(option2Success);

            }
            else
            {
                ExperienceManager.Instance.RemoveXP(option2Fail);
            }
        }
        void OnOption3()
        {
            ExperienceManager.Instance.RemoveXP(option3);
        }
    }
}