using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using NUnit.Framework;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GopnikEvent : MonoBehaviour
{
    public static GopnikEvent Instance;
    public EventPopUpUI popUpUI;
    public Image backgroundImage;
    public Image robberImage;
    public float eventChance = 1.0f;
    void Awake()
    {
        Instance = this;
        Debug.Log("GopnikEvent Awake ran");
    }
    public void DropIsPicked()
    {
        Debug.Log("aga");
        float roll = Random.Range(0f, 1.0f);
        if(roll <= eventChance)
        {
            StartGopnikEvent();
        }
    }
    private void StartGopnikEvent()
    {
        popUpUI.Show(
            "Title",
            backgroundImage,
            robberImage,
            "Pisi i barzda",
            () =>
            {
                Debug.Log("Pisai i barzda");
            },
            "Bandai pabegt",
            () =>
            {
                Debug.Log("pabegai");
            },
            "atiduok dropus",
            () =>
            {
                Debug.Log("atidavei dropukus");
            }

        );
    }
}