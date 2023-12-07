using System.Collections;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace HealthSystem
{
    public enum CharacterName
    {
        Player,
        Orc,
        Wolf,
        Golem
    }
    public class HealthSystem : MonoBehaviour
    {
        #region Declare Variables
        [SerializeField] protected Slider sliderHpPlayer;
        [SerializeField] public float maxHp;
        public float CurrentHp { get; private set; }
        [HideInInspector] public bool isDead;
        protected Animator animator;
        protected SpriteRenderer spriteRenderer;
        public GameObject damageIndicator;

        [Header("Sound")]
        public AudioClip[] takeDamageSounds;
        [SerializeField] protected AudioClip[] deadSounds;
        #endregion
    
        private void Awake()
        {
            CurrentHp = maxHp;
        }
        protected virtual void Start()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            TryGetComponent(out animator);
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
            if (isDead) return;
            ShowDamageIndicator(damage);
            CurrentHp -= damage;
            CurrentHp = Mathf.Clamp(CurrentHp, 0, maxHp);
            UpdateUI();
        
            /*if(_animator)
                _animator.SetTrigger(TakDamage);*/
            spriteRenderer.color = new Color(1,0.6f,0.6f,1);
            Invoke(nameof(ResetSpriteColor), 0.2f);
        
            if (CurrentHp > 0 || isDead) return;
            Dead();
        }

        
        /// <summary>
        /// Heal and increase the current hp.
        /// </summary>
        /// <param name="healPoint">Heal amount.</param>
        public virtual  void Heal(float healPoint)
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
            isDead = true;
            SoundManager.Instance.RandomPlaySound(deadSounds);
            StartCoroutine(WaitForDeadAnimation());
        }
        
        IEnumerator WaitForDeadAnimation()
        {
            if (animator)
                animator.SetTrigger("Dead");
            float timeCount = 0;
            while (timeCount < 1f)
            {
                Color color = spriteRenderer.color;
                color.a = 1 - timeCount;
                spriteRenderer.color = color;
                timeCount += Time.deltaTime;
                yield return null;
            }
            if(this is PlayerHealthSystem)
                gameObject.SetActive(false);
            else
                Destroy(gameObject);
        }
    
        
        /// <summary>
        /// Reset the current hp to max hp.
        /// </summary>
        public void ResetHealth()
        {
            isDead = false;
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

        protected void ResetSpriteColor()
        {
            spriteRenderer.color = Color.white;
        }
        
        protected virtual void ShowDamageIndicator(float damage)
        {
            if (!damageIndicator) return;
            Vector3 offset = new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f),0);
            var go = Instantiate(damageIndicator, transform.position + offset, quaternion.identity);
            go.GetComponent<TextMeshPro>().text = damage.ToString("0");
        }
        #endregion
    }
}
