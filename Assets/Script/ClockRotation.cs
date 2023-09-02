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
    private DayAndNight _dayAndNight;
    private int dayInt = 1;

    void Start()
    {
        currentTime = 0;
        _dayAndNight = GetComponent<DayAndNight>();
    }

    // Update is called once per frame
    void Update()
    {
        currentTime = currentTime + Time.deltaTime;
        if (currentTime >= _dayAndNight.cycleLength)
        {
            dayInt += 1;
            currentTime = 0;
        }
        clock.rectTransform.rotation = Quaternion.Euler(0,0,(currentTime / _dayAndNight.cycleLength )*360);

        day.text = "Day " + Convert.ToString(dayInt);
    }
    
    
}
