using UnityEngine;
using UnityEngine.UI;

public class HealthSystem : MonoBehaviour
{
    [SerializeField] private Slider sliderHpPlayer;
    [SerializeField] private float maxHp;
    private float _currentHp;
    
    void Start()
    {
        _currentHp = maxHp;
    }
    
    void Update()
    {

    }
    
    public void TakeDamage(float damage)
    {
        _currentHp -= damage;
        sliderHpPlayer.value = _currentHp / maxHp;

        if (_currentHp <= 0)
        {
            // die
        }

    }
    public void Heal(float damage)
    {
        _currentHp += damage;
        sliderHpPlayer.value = _currentHp / maxHp;
    }
}
