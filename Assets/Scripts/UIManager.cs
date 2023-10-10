using System.Collections;
using System.Collections.Generic;
using HealthSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private HealthSystem.HealthSystem Base;
    [SerializeField] private HealthSystem.HealthSystem Player;
    [SerializeField] private TextMeshProUGUI BaseHPText;
    [SerializeField] private TextMeshProUGUI PlayerHPText;
    [SerializeField] private TextMeshProUGUI PlayerStaminaText;
    private float baseHP;
    private float baseMaxHP;
    [SerializeField] private Slider baseHPSlider;
    [SerializeField] private Slider playerStaminaSlider;
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        baseHPSlider.value = Base.CurrentHp / Base.maxHp;
        BaseHPText.text = $"{Base.CurrentHp} / {Base.maxHp}";
        PlayerHPText.text = $"{Player.CurrentHp} / {Player.maxHp}";
        //PlayerStaminaText.text = $"{Base.CurrentHp} / {Base.maxHp}"; stamina
        
    }
}
