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
    private TimeSystem _timeSystem;
    private int dayInt = 1;

    void Start()
    {
        currentTime = 0;
        _timeSystem = GetComponent<TimeSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        currentTime = currentTime + Time.deltaTime;
        if (currentTime >= _timeSystem.cycleLength)
        {
            dayInt += 1;
            currentTime = 0;
        }
        clock.rectTransform.rotation = Quaternion.Euler(0,0,(currentTime / _timeSystem.cycleLength )*360);

        day.text = "Day " + Convert.ToString(dayInt);
    }
    
    
}
