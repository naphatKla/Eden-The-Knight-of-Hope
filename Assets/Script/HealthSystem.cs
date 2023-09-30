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
        sliderHpPlayer.value = _currentHp / maxHp;
    }
    
    public virtual void TakeDamage(float damage)
    {
        if(_animator != null)
            _animator.SetTrigger("TakeDamage");
        
        _currentHp -= damage;

        if (_currentHp <= 0)
            Dead();
    }

    protected virtual void Dead()
    {
        if (gameObject.tag.Equals("Player"))
        {
            if (!gameObject.activeSelf) return;
            gameObject.SetActive(false);
            Invoke(nameof(Respawn), 5);
        }
        else
            Destroy(gameObject);
    }

    public void Heal(float Heal)
    {
        _currentHp += Heal;
    }

    public void FullHeal()
    {
        _currentHp = maxHp;
    }
    
    public void Respawn()
    {
        transform.position = GameManager.instance.spawnPoint;
        gameObject.SetActive(true);
        gameObject.GetComponent<Player>().Reset();
        _currentHp = maxHp;
        sliderHpPlayer.value = _currentHp / maxHp;
    }
}
