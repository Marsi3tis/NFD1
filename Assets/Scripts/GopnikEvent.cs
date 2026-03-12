using UnityEngine;
using UnityEngine.UI;
using Unity.Cinemachine;
using UnityEditor.PackageManager.Requests;
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
    public CinemachineCamera cmCamera;
    public EventPopUpUI popUpUI;
    public Image backgroundImage;
    public Image robberImage;
    public float eventChance = 1.0f;

    bool EventEnding;
    void Awake()
    {
        
        Instance = this;
    }
    public void DropIsPicked()
    {
        float roll = Random.Range(0f, 1.0f);
        if(roll <= eventChance)
        {
            TopControl.Instance.StopTheCar();
            cmCamera.PreviousStateIsValid = false;
        
            
            StartGopnikEvent();
        }
    }
    private void EndGopnikEvent()
    {
        
        if(EventEnding == true)
        {
            popUpUI.Show(
                "Title",
                "LALALALAL111111111",
                backgroundImage,
                robberImage,
                "Continue",
                () =>
                {
                    popUpUI.Hide();
                }

        );
        }

        
    }
    private void StartGopnikEvent()
    {
        popUpUI.Show(
            "Title",
            "LALALALAL",
            backgroundImage,
            robberImage,
            "Pisi i barzda",
            () =>
            {
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

        );}
        void OnOption1()
        {
            float roll = Random.Range(0f, 1.0f);
            if(roll <= option1)
            {
                ExperienceManager.Instance.AddXP(option1Success);
                EventEnding = true;
                EndGopnikEvent();
            }
            else
            {
                ExperienceManager.Instance.RemoveXP(option1Fail);
                EventEnding = false;
            }
        }
        void OnOption2()
        {
            float roll = Random.Range(0f, 1.0f);
            if(roll <= option2)
            {
                ExperienceManager.Instance.AddXP(option2Success);
                EventEnding = true;
                EndGopnikEvent();
                
            }
            else
            {
                ExperienceManager.Instance.RemoveXP(option2Fail);
                EventEnding = false;                
            }
        }
        void OnOption3()
        {
            ExperienceManager.Instance.RemoveXP(option3);
            EventEnding = false;
        }
}