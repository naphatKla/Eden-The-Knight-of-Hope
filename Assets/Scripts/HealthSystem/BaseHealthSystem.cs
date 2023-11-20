using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HealthSystem
{
    public class BaseHealthSystem : HealthSystem
    {
        [SerializeField] private float hpRegenPercentage;
        public static BaseHealthSystem Instance;

        protected override void Start()
        {
            base.Start();
            Instance = this;
        }

        private void Update()
        {
            // regen hp when day time.
            sliderHpPlayer.gameObject.SetActive(CurrentHp < maxHp);
            if(TimeSystem.Instance.timeState != TimeState.Day) return;
            Heal((hpRegenPercentage / 100) * maxHp * Time.deltaTime);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!(other.CompareTag("Enemy") || other.CompareTag("Player"))) return;
            StartCoroutine(LerpAlpha(GetComponent<SpriteRenderer>(), 0.75f, 0.5f));
        }
        
        private void OnTriggerExit2D(Collider2D other)
        {
            if (!(other.CompareTag("Enemy") || other.CompareTag("Player"))) return;
            StartCoroutine(LerpAlpha(GetComponent<SpriteRenderer>(), 1f, 0.5f));
        }
    
        private IEnumerator LerpAlpha(SpriteRenderer spriteRenderer,float destination, float time)
        {
            float timeCout = 0;
            while (timeCout < time)
            {
                Color color = spriteRenderer.color;
                color.a = Mathf.Lerp(color.a, destination, timeCout / time);
                spriteRenderer.color = color;
                timeCout += Time.deltaTime;
                yield return null;
            }
        }
    }
}
