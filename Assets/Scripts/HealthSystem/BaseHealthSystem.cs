using System.Linq;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace HealthSystem
{
    public class BaseHealthSystem : HealthSystem
    {
        [SerializeField] private float hpRegenPercentage;
        [SerializeField] private LayerMask alphaLayerMask;
        [SerializeField] private Image hpFill;
        public static BaseHealthSystem Instance;
        private SpriteRenderer _spriteRenderer;

        protected override void Start()
        {
            base.Start();
            Instance = this;
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void Update()
        {
            AlphaDetect();
            // regen hp when day time.
            sliderHpPlayer.gameObject.SetActive(CurrentHp < maxHp);
            if(TimeSystem.Instance.timeState != TimeState.Day) return;
            Heal((hpRegenPercentage / 100) * maxHp * Time.deltaTime);
        }

        public override void TakeDamage(float damage, GameObject attacker = null)
        {
            hpFill.color = CurrentHp / maxHp > 0.5f
                ? Color.Lerp(Color.green, Color.yellow, (1 - (CurrentHp / maxHp)) * 2)
                : Color.Lerp(Color.yellow, Color.red, (1 - (CurrentHp / (maxHp / 2))));
            
            Color color = hpFill.color;
            color.a = 0.75f;
            hpFill.color = color;
            base.TakeDamage(damage, attacker);
        }

        private void AlphaDetect()
        {
            Collider2D[] objInSprite = Physics2D.OverlapBoxAll(transform.position + _spriteRenderer.sprite.bounds.center,
                _spriteRenderer.sprite.bounds.size + new Vector3(-2,-1,0), 0, alphaLayerMask);
            if (objInSprite.Length > 0)
            {
                if (objInSprite.Any(obj => obj.transform.position.y > transform.position.y+1f))
                {
                    if (_spriteRenderer.color.a > 0.75f)
                    {
                        Color color = _spriteRenderer.color;
                        color.a -= 0.01f;
                        _spriteRenderer.color = color;
                    }
                }
                else if (_spriteRenderer.color.a < 1)
                {
                    Color color = _spriteRenderer.color;
                    color.a += 0.01f;
                    _spriteRenderer.color = color;
                }
            }
            else if (_spriteRenderer.color.a < 1)
            {
                Color color = _spriteRenderer.color;
                color.a += 0.01f;
                _spriteRenderer.color = color;
            }
        }
        
        protected override void ShowDamageIndicator(float damage)
        {
            if (!damageIndicator) return;
            Vector3 offset = new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f),0);
            var go = Instantiate(damageIndicator, transform.position + offset, quaternion.identity);
            go.GetComponent<TextMeshPro>().text = $"<color=red>{damage}";
        }
    }
}
