using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
/*using Input = UnityEngine.Windows.Input;*/

public class HealthSystem : MonoBehaviour
{
    //Health System
    [Header("Player Health")] 
    [SerializeField] private Slider sliderHpPlayer;
    [FormerlySerializedAs("maxHpPlayer")] [SerializeField] private float maxHp;
    private float _currentHp;
    
    
    
    
    // Start is called before the first frame update
    void Start()
    {
        _currentHp = maxHp;
    }

    // Update is called once per frame
    void Update()
    {
        if (_currentHp <= 0)
        {
            Debug.Log("Die");
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            TakeDamage(10);
        }
    }

    // เผื่อไว้
    public void TakeDamage(float damage)
    {
        _currentHp -= damage;
        sliderHpPlayer.value = _currentHp / maxHp;
    }
    public void Heal(float damage)
    {
        _currentHp += damage;
        sliderHpPlayer.value = _currentHp / maxHp;
    }
}
