using UnityEngine;
using UnityEngine.UI;

public class HealthSystem : MonoBehaviour
{
    [SerializeField] private Slider sliderHpPlayer;
    [SerializeField] public float maxHp;
    public float _currentHp;
    protected Animator _animator;

    protected virtual void Start()
    {
        _currentHp = maxHp;
        _animator = GetComponent<Animator>();
    }
    
    protected virtual void Update()
    {
        _currentHp = Mathf.Clamp(_currentHp, 0, maxHp);
    }
    
    public virtual void TakeDamage(float damage)
    {
        if(_animator != null)
            _animator.SetTrigger("TakeDamage");
        
        _currentHp -= damage;
        sliderHpPlayer.value = _currentHp / maxHp;

        if (_currentHp <= 0)
            Dead();

    }

    protected virtual void Dead()
    {
        if (gameObject.tag.Equals("Player"))
        {
            if (!gameObject.activeSelf) return;
            gameObject.SetActive(false);
            Invoke(nameof(Respawn), 8);
        }
        else
            Destroy(gameObject);
    }

    public void Heal(float damage)
    {
        _currentHp += damage;
        sliderHpPlayer.value = _currentHp / maxHp;
    }

    public void FullHeal()
    {
        _currentHp = maxHp;
        sliderHpPlayer.value = _currentHp / maxHp;
    }
    
    public void Respawn()
    {
        transform.position = GameManager.instance.playerBase.position;
        gameObject.SetActive(true);
        gameObject.GetComponent<Player>().ResetState();
        _currentHp = maxHp;
        sliderHpPlayer.value = _currentHp / maxHp;
    }
}
