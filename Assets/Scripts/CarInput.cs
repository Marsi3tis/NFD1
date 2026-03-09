using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarInput : MonoBehaviour
{
    //Components
    TopControl topControl;

    void Awake()
    {
        topControl = GetComponent<TopControl>();
    }
    void Update()
    {
        
        Vector2 inputVector = Vector2.zero;
        inputVector.x = Input.GetAxis("Horizontal");
        inputVector.y = Input.GetAxis("Vertical");

        topControl.SetInputVector(inputVector);
        if (Input.GetKey(KeyCode.Space))
            topControl.SetBrake(true);
        else topControl.SetBrake(false);
    }
}

