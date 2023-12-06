using System.Collections;
using System.Linq;
using PlayerBehavior;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Interaction
{
    public class InteractableObject : MonoBehaviour
    {
        #region Declare Variables
        [SerializeField] protected KeyCode key;
        [TextArea] [SerializeField] protected string prompt;
        [SerializeField] protected TextMeshProUGUI interactionTextUI;
        [SerializeField] protected GameObject[] interactionIndicators;
        [SerializeField] protected float countdownTime;
        [SerializeField] public Slider progressBar;
        protected Coroutine interactCoroutine;
        protected SpriteRenderer SpriteRenderer;
        [SerializeField] private LayerMask alphaLayerMask;
        #endregion

        protected virtual void Start()
        {
            interactionTextUI.text = prompt;
            SpriteRenderer = GetComponent<SpriteRenderer>();
        }

        protected virtual void Update()
        {
            AlphaDetect();
        }
        
        #region Methods
        /// <summary>
        /// Event when the object is targeted.
        /// </summary>
        /// <param name="isTarget">Is player target this object or not.</param>
        public virtual void OnTarget(bool isTarget)
        {
            foreach (var interactionIndicator in interactionIndicators)
                interactionIndicator.SetActive(isTarget);
        
            if (!isTarget) return;
            InteractHandler();
        }

        
        /// <summary>
        /// Condition and interaction logic.
        /// </summary>
        protected virtual void InteractHandler()
        {
            // Pls override this method
            // Do something when interact
            if(!Input.GetKeyDown(key)) return;
            if(interactCoroutine != null) return;
            interactCoroutine = StartCoroutine(CountDownAndInteract(countdownTime));
        }
    
        
        /// <summary>
        /// Action of the interaction.
        /// </summary>
        protected virtual void InteractAction()
        {
            Destroy(gameObject);
        }
    
        private void AlphaDetect()
        {
            Collider2D[] objInSprite = Physics2D.OverlapBoxAll(transform.position + SpriteRenderer.sprite.bounds.center,
                SpriteRenderer.sprite.bounds.size, 0, alphaLayerMask);
            if (objInSprite.Length > 0)
            {
                if (objInSprite.Any(obj => obj.transform.position.y > transform.position.y))
                {
                    if (SpriteRenderer.color.a > 0.75f)
                    {
                        Color color = SpriteRenderer.color;
                        color.a -= 0.01f;
                        SpriteRenderer.color = color;
                    }
                }
                else if (SpriteRenderer.color.a < 1)
                {
                    Color color = SpriteRenderer.color;
                    color.a += 0.01f;
                    SpriteRenderer.color = color;
                }
            }
            else if (SpriteRenderer.color.a < 1)
            {
                Color color = SpriteRenderer.color;
                color.a += 0.01f;
                SpriteRenderer.color = color;
            }
        }
        
        // ReSharper disable Unity.PerformanceAnalysis
        /// <summary>
        /// Performs a countdown timer and triggers an interaction when the timer reaches zero.
        /// </summary>
        /// <param name="time">Countdown time before interact.</param>
        protected virtual IEnumerator CountDownAndInteract(float time)
        {
            float progressionTimeLeft = time;
            progressBar.gameObject.SetActive(true);
        
            while (progressionTimeLeft > 0)
            {
                progressBar.value = progressionTimeLeft / time;
                progressionTimeLeft -= Time.deltaTime;
                yield return null;

                PlayerInteractSystem.Instance.isStopMove = time - progressionTimeLeft <= 0.3f;
                if (progressionTimeLeft < 0.15f) continue;
                if(Input.GetAxisRaw("Horizontal") == 0 && Input.GetAxisRaw("Vertical") == 0) continue;
                if (PlayerInteractSystem.Instance.isStopMove) continue;
                progressBar.gameObject.SetActive(false);
                interactCoroutine = null;
                PlayerInteractSystem.Instance.isStopMove = false;
                yield break;
            }
        
            InteractAction();
            progressBar.gameObject.SetActive(false);
            interactCoroutine = null;
            progressBar.gameObject.SetActive(false);
            PlayerInteractSystem.Instance.isStopMove = false;
        }
        #endregion
    }
}
