using UnityEngine;
using UnityEngine.UI;

public class HealthSystem : MonoBehaviour
{
    [SerializeField] private Slider sliderHpPlayer;
    [SerializeField] private float maxHp;
    private float _currentHp;
    private Animator _animator;

    void Start()
    {
        _currentHp = maxHp;
        _animator = GetComponent<Animator>();
    }
    
    void Update()
    {
        
    }
    
    public void TakeDamage(float damage)
    {
        if(_animator != null)
            _animator.SetTrigger("TakeDamage");
        
        _currentHp -= damage;
        sliderHpPlayer.value = _currentHp / maxHp;

        if (_currentHp <= 0)
        {
            gameObject.SetActive(false);
        }

    }
    public void Heal(float damage)
    {
        _currentHp += damage;
        sliderHpPlayer.value = _currentHp / maxHp;
    }
}
