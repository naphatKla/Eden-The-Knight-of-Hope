using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class TimeSystem : MonoBehaviour
{
    public enum TimeState
    {
        Day,
        Night,
    }
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
    public TimeState timeState;

    public static TimeSystem instance;

    void Start()
    {
        day = 1;
        cycleLength = dayTime + nightTime;
        _lightUpDuration = lightUpPeriod.y - lightUpPeriod.x;
        _lightDownDuration = lightDownPeriod.y - lightDownPeriod.x;
        time = ConvertHourToSec(gameStartTime);
        instance = this;
    }
    
    void Update()
    {
        DayTimeUpdate();
        DayLightHandle();
        
        clock.rectTransform.rotation = Quaternion.Euler(0,0,(time / cycleLength )*720);
    }

    private void DayTimeUpdate()
    {
        time += Time.deltaTime * _timeMultiplier;
        dayText.text = $"Day {day}\n {ConvertSecToHour(time):F0}:00";

        if (TimePeriodCheck(dayPeriod.x,dayPeriod.y))
        {
            timeState = TimeState.Day;
            _timeMultiplier = 1;
        }
        else
        {
            timeState = TimeState.Night;
            _timeMultiplier = dayTime / nightTime;
        }
        
        if (time < cycleLength) return;
        time = 0;
        day += 1;
    }
    
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
    
    public float ConvertSecToHour(float sec)
    {
        return sec / (3600 / (86400 / cycleLength));
    }
    
    public float ConvertHourToSec(float hour)
    {
        if (timeState == TimeState.Night)
            _timeMultiplier = dayTime / nightTime;
        else
            _timeMultiplier = 1;
        
        return hour * (3600 / (86400 / cycleLength));
    }

    public bool TimePeriodCheck(float x, float y)
    {
        return ConvertSecToHour(time) >= x && ConvertSecToHour(time) <= y;
    }

    public float GetCurrentTime()
    {
        return ConvertSecToHour(time);
    }

    public TimeState GetTimeState()
    {
        return timeState;
    }
}
