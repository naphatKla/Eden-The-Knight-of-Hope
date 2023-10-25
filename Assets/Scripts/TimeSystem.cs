using System;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public enum TimeState
{
    Day,
    Night,
}

public class TimeSystem : MonoBehaviour
{
    #region MyRegion
    [Header("Time")]
    public float dayTime;
    public float nightTime;
    public float gameStartTime;
    [SerializeField] private Vector2 dayPeriod;
    [HideInInspector] public float cycleLength;
    [HideInInspector] public float time;
    [HideInInspector] public int day;
    private float _timeMultiplier;
    
    [Header("Light")] 
    [SerializeField] private Light2D dayLight;
    [SerializeField] [Range(0, 1)] private float maxLight;
    [SerializeField] [Range(0,1)] private float minLight;
    [SerializeField] private Vector2 lightUpPeriod;
    [SerializeField] private Vector2 lightDownPeriod;
    private float _lightUpDuration;
    private float _lightDownDuration;
    
    [Header("Time UI")]
    [SerializeField] private Image clock;
    [SerializeField] private TextMeshProUGUI dayText;
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private Image dayAndNightIcon;
    [SerializeField] private Sprite dayIcon;
    [SerializeField] private Sprite nightIcon;
    public TimeState timeState;
    public event Action OnNewDay, OnDay, OnNight;
    public static TimeSystem Instance;
    #endregion

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        day = 0;
        cycleLength = dayTime + nightTime;
        _lightUpDuration = lightUpPeriod.y - lightUpPeriod.x;
        _lightDownDuration = lightDownPeriod.y - lightDownPeriod.x;
        time = ConvertHourToSec(gameStartTime);
    }
    
    private void Update()
    {
        TimeUpdateHandler();
        DayLightHandle();

        clock.rectTransform.rotation = Quaternion.Euler(0, 0, (time / cycleLength) * 360);
    }

    #region Methoos
    // ReSharper disable Unity.PerformanceAnalysis
    /// <summary>
    /// Update time and day.
    /// </summary>
    private void TimeUpdateHandler()
    {
        UpdateTimeMultiplier();
        time += Time.deltaTime * _timeMultiplier;
        dayText.text = $"Day {day}";
        timeText.text = $"{Mathf.Floor(GetCurrentTime()):F0}:00";
        
        // new day
        if (time < cycleLength) return;
        time = 0;
        day += 1;
        OnNewDay?.Invoke();
    }
    
    
    /// <summary>
    /// Update time multiplier and time state.
    /// </summary>
    private void UpdateTimeMultiplier()
    {
        const float dayTimeMultiplier = 1;
        float nightTimeMultiplier = dayTime / nightTime;
        
        if (TimePeriodCheck(dayPeriod.x, dayPeriod.y))
        {
            if (TimeSystem.Instance.timeState == TimeState.Day) return;
            _timeMultiplier = dayTimeMultiplier;
            timeState = TimeState.Day;
            dayAndNightIcon.sprite = dayIcon;
            OnDay?.Invoke();
        }
        else
        {
            if (TimeSystem.Instance.timeState == TimeState.Night) return;
            _timeMultiplier = nightTimeMultiplier;
            timeState = TimeState.Night;
            dayAndNightIcon.sprite = nightIcon;
            OnNight?.Invoke();
        }
    }

    
    /// <summary>
    /// Handle day light intensity.
    /// </summary>
    private void DayLightHandle()
    {
        float lightDownTimeCounter= ConvertSecToHour(time) - lightDownPeriod.x ,
              lightUpTimeCounter = ConvertSecToHour(time) - lightUpPeriod.x ;
 
        if (TimePeriodCheck(lightDownPeriod.x, lightDownPeriod.y))
            dayLight.intensity = Mathf.Lerp(maxLight, minLight,
                lightDownTimeCounter / _lightDownDuration);
        
        else if (TimePeriodCheck(lightUpPeriod.x, lightUpPeriod.y))
            dayLight.intensity = Mathf.Lerp(minLight, maxLight,
                lightUpTimeCounter / _lightUpDuration);
    }
    
    
    /// <summary>
    /// Check if current time is in the period of x time and y time or not.
    /// </summary>
    /// <param name="x">Start time period (hour).</param>
    /// <param name="y">End time period (hour).</param>
    /// <returns></returns>
    public bool TimePeriodCheck(float x, float y)
    {
        return GetCurrentTime() >= x && GetCurrentTime() <= y;
    }
    
    
    /// <summary>
    /// Convert second to hour.
    /// </summary>
    /// <param name="sec">time to convert (sec).</param>
    /// <returns>time converted (hour)</returns>
    private float ConvertSecToHour(float sec)
    {
        return sec / (3600 / (86400 / cycleLength));
    }

    
    /// <summary>
    /// Convert hour to second.
    /// </summary>
    /// <param name="hour">time to convert (hour).</param>
    /// <returns>time converted (sec)</returns>
    private float ConvertHourToSec(float hour)
    {
        if (timeState == TimeState.Night)
            _timeMultiplier = dayTime / nightTime;
        else
            _timeMultiplier = 1;
        
        return hour * (3600 / (86400 / cycleLength));
    }
    

    /// <summary>
    /// Get current time (hour). 
    /// </summary>
    /// <returns>Current Time (hour).</returns>
    private float GetCurrentTime()
    {
        return ConvertSecToHour(time);
    }
    #endregion
}
