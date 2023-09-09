using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

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
        if (Input.GetKeyDown(KeyCode.E))
        {
            TakeDamage(15);
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Heal(15);
        }

        if (_currentHp <= 0)
        {
            Debug.Log("Die");
        }
        
    }

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
