using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class HealthSystem : MonoBehaviour
{
    #region Declare Variables
    [SerializeField] protected Slider sliderHpPlayer;
    [SerializeField] public float maxHp;
    public float CurrentHp { get; protected set; }
    private bool _isDead;
    private Animator _animator;
    #endregion
    
    protected virtual void Start()
    {
        CurrentHp = maxHp;
        TryGetComponent(out _animator);
    }

    private void LateUpdate()
    {
        // Lock the canvas UI rotation.
        Transform canvasTransform = sliderHpPlayer.transform.parent;
        canvasTransform.right = Vector3.right;
    }

    #region Methods
    // ReSharper disable Unity.PerformanceAnalysis
    /// <summary>
    /// Take damage and reduce the current hp.
    /// </summary>
    /// <param name="damage">Damage taken.</param>
    /// <param name="attacker">Attacker.</param>
    public virtual void TakeDamage(float damage, GameObject attacker = null)
    {
        CurrentHp -= damage;
        CurrentHp = Mathf.Clamp(CurrentHp, 0, maxHp);
        UpdateUI();
        
        if(!_animator.IsUnityNull())
            _animator.SetTrigger("TakeDamage");
        
        if (CurrentHp > 0 || _isDead) return;
        _isDead = true;
        Dead();
    }

    /// <summary>
    /// Heal and increase the current hp.
    /// </summary>
    /// <param name="healPoint">Heal amount.</param>
    public void Heal(float healPoint)
    {
        CurrentHp += healPoint;
        CurrentHp = Mathf.Clamp(CurrentHp, 0, maxHp);
        UpdateUI();
    }
    
    /// <summary>
    /// Dead and destroy the object.
    /// </summary>
    protected virtual void Dead()
    {
        Destroy(gameObject);
    }
    
    /// <summary>
    /// Reset the current hp to max hp.
    /// </summary>
    public void ResetHealth()
    {
        _isDead = false;
        CurrentHp = maxHp;
        UpdateUI();
    }
    
    /// <summary>
    /// Update the UI.
    /// </summary>
    private void UpdateUI()
    {
        sliderHpPlayer.value = CurrentHp / maxHp;
    }
    #endregion
}
