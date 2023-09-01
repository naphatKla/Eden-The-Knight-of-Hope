using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ClockRotation : MonoBehaviour
{
    private float currentTime;
    [SerializeField] private Image clock;
    [SerializeField] private TMP_Text day;
    private int dayInt = 1;

    void Start()
    {
        currentTime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        currentTime = currentTime + Time.deltaTime;
        if (currentTime >= 800)
        {
            dayInt += 1;
            currentTime = 0;
        }
        clock.rectTransform.rotation = Quaternion.Euler(0,0,(currentTime / 800 )*360);

        day.text = "Day " + Convert.ToString(dayInt);
    }
    
    
}
