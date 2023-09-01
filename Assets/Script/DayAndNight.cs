using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class DayAndNight : MonoBehaviour
{
    [System.Serializable]
    public struct DayAndNightMark
    {
        public float timeRatio;
        public Color colors;
    }
    
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private DayAndNightMark[] marks;
    float cycleLength ; //1 day time / sec

    private const float TIME_CHECK_EPSILON = 0.1f;

    private float currentCycleTime = 1f;
    private int currentMarkIndex, nextMarkIndex;
    private float currentMarkTime, nextMarkTime;
    private float currentTimeForClock;
    private float targetPoint;
    private float colorTransitionTime = 1f;

     void Start()
     {
         currentMarkIndex = 0;
         nextMarkIndex = 1;
     }

     void Update()
     {
         currentCycleTime += Time.deltaTime;
         if (currentCycleTime >= 20)
         {
             targetPoint += Time.deltaTime / colorTransitionTime;                                                    
             sprite.color = Color.Lerp(marks[currentMarkIndex].colors, marks[nextMarkIndex].colors, targetPoint);    
             if (targetPoint >= 1f)                                                                                  
             {                                                                                                       
                 targetPoint = 0f;                                                                                   
             }                                                                                                       
         }
         if (currentCycleTime >= marks[currentMarkIndex].timeRatio)
         {
             currentCycleTime = 0;
             currentMarkIndex = nextMarkIndex;
             nextMarkIndex += 1;
             if (nextMarkIndex == marks.Length-1)
             {
                 nextMarkIndex = 0;
             }
         }
         Debug.Log(currentCycleTime);
         Debug.Log(targetPoint);
         
     }
}