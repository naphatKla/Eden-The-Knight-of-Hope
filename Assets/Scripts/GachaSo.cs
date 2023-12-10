using CombatSystem;
using UnityEngine;

public enum Grade
{
    Common,
    Uncommon,
    Rare,
    Epic,
    Legendary,
    Mythic,
}

public struct GradeColor
{
    public static Color Common = new Color(0.5f, 0.5f, 0.5f);
    public static Color Uncommon = new Color(0.2f, 0.8f, 0.2f);
    public static Color Rare = new Color(0.2f, 0.2f, 0.8f);
    public static Color Epic = new Color(0.8f, 0.2f, 0.8f);
    public static Color Legendary = new Color(0.8f, 0.8f, 0.2f);
    public static Color Mythic = new Color(0.8f, 0.2f, 0.2f);
    
    public static Color GetGradeColor(Grade grade)
    {
        switch (grade)
        {
            case Grade.Common:
                return Common;
            case Grade.Uncommon:
                return Uncommon;
            case Grade.Rare:
                return Rare;
            case Grade.Epic:
                return Epic;
            case Grade.Legendary:
                return Legendary;
            case Grade.Mythic:
                return Mythic;
            default:
                return Color.white;
        }
    }
}

[CreateAssetMenu]
public class GachaSo : ScriptableObject
{
    [SerializeField] private string gachaName;
    [SerializeField] private float atkStatsPercentage;
    [SerializeField] private float atkSpeedStatsPercentage;
    [SerializeField] private Grade grade;
    private string _gachaNameWithHexColor;
    public string GachaName => gachaName;
    public string GachaNamWithHexColor => _gachaNameWithHexColor;
    
    public void InitializeName()
    {
        Color gradeColor = GradeColor.GetGradeColor(grade);
        string gradeColorHex = ColorUtility.ToHtmlStringRGB(gradeColor);
        _gachaNameWithHexColor = $"<color=#{gradeColorHex}>{grade} {gachaName}</color>";
    }

    public void AddStat()
    {
        PlayerCombatSystem.Instance.AtkPercent += atkStatsPercentage / 100;
        PlayerCombatSystem.Instance.ReduceCoolDownPercent += (atkSpeedStatsPercentage/100);
    }
    
    public void RemoveStat()
    {
        PlayerCombatSystem.Instance.AtkPercent -= atkStatsPercentage / 100;
        PlayerCombatSystem.Instance.ReduceCoolDownPercent -= (atkSpeedStatsPercentage/100);
    }
    
    public string GetDescription()
    {
        string str1 = atkStatsPercentage < 0? $"ATK: {atkStatsPercentage}%" : $"ATK: +{atkStatsPercentage}%";
        string str2 = atkSpeedStatsPercentage < 0? $"ATK Speed: {atkSpeedStatsPercentage}%" : $"ATK Speed: +{atkSpeedStatsPercentage}%";
        return str1+"\n"+str2;
    }
}
