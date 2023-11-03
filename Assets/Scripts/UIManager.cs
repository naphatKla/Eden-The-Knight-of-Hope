using HealthSystem;
using PlayerBehavior;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Slider playerBaseHpSlider;
    [SerializeField] private Slider playerHpSlider;
    [SerializeField] private Slider playerStaminaSlider;
    [Space]
    [SerializeField] private TextMeshProUGUI playerBaseHpText;
    [SerializeField] private TextMeshProUGUI playerHpText;
    [SerializeField] private TextMeshProUGUI playerStaminaText;
    public static UIManager Instance;
    public bool isAnyUIOpen;
    
    private void Start()
    {
        Instance = this;
    }

    private void Update()
    {
        playerBaseHpSlider.value = BaseHealthSystem.Instance.CurrentHp / BaseHealthSystem.Instance.maxHp;
        playerHpSlider.value = PlayerHealthSystem.Instance.CurrentHp / PlayerHealthSystem.Instance.maxHp;
        playerStaminaSlider.value = Player.Instance.CurrentStamina/ Player.Instance.MaxStamina;
        
        playerBaseHpText.text = $"{BaseHealthSystem.Instance.CurrentHp:F0} / {BaseHealthSystem.Instance.maxHp:F0}";
        playerHpText.text = $"{PlayerHealthSystem.Instance.CurrentHp} / {PlayerHealthSystem.Instance.maxHp}";
        playerStaminaText.text = $"{Player.Instance.CurrentStamina:F0} / {Player.Instance.MaxStamina:F0}";
    }
}
