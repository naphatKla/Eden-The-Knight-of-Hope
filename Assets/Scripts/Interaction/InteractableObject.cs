using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Interaction
{
    public class InteractableObject : MonoBehaviour
    {
        #region MyRegion
        [SerializeField] protected KeyCode key;
        [SerializeField] protected int point; // maybe change to any item in future
        [SerializeField] protected string prompt;
        [SerializeField] protected TextMeshProUGUI interactionTextUI;
        [SerializeField] protected GameObject[] interactionIndicators;
        [SerializeField] protected float countdownTime;
        [SerializeField] public Slider progressBar;
        private Coroutine _interactCoroutine;
        #endregion

        protected virtual void Start()
        {
            interactionTextUI.text = prompt;
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
            if(_interactCoroutine != null) return;
            _interactCoroutine = StartCoroutine(CountDownAndInteract(countdownTime));
        }
    
        
        /// <summary>
        /// Action of the interaction.
        /// </summary>
        protected virtual void InteractAction()
        {
            GameManager.Instance.AddPoint(point);
            Destroy(gameObject);
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
            
                if (progressionTimeLeft < 0.15f) continue;
                if(Input.GetAxisRaw("Horizontal") == 0 && Input.GetAxisRaw("Vertical") == 0) continue;
                progressBar.gameObject.SetActive(false);
                _interactCoroutine = null;
                yield break;
            }
        
            InteractAction();
            progressBar.gameObject.SetActive(false);
            _interactCoroutine = null;
        }
        #endregion
    }
}
