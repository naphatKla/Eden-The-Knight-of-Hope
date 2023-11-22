using System.Linq;
using UnityEngine;

namespace HealthSystem
{
    public class BaseHealthSystem : HealthSystem
    {
        [SerializeField] private float hpRegenPercentage;
        [SerializeField] private LayerMask alphaLayerMask;
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

        private void AlphaDetect()
        {
            Collider2D[] objInSprite = Physics2D.OverlapBoxAll(transform.position + _spriteRenderer.sprite.bounds.center,
                _spriteRenderer.sprite.bounds.size + new Vector3(-2,-1,0), 0, alphaLayerMask);
            if (objInSprite.Length > 0)
            {
                if (objInSprite.Any(obj => obj.transform.position.y > transform.position.y+1f))
                {
                    if (_spriteRenderer.color.a > 0.5f)
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
    }
}
