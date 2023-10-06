using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class InteractableObject : MonoBehaviour
{
    [SerializeField] protected KeyCode key;
    [SerializeField] protected int point;
    [SerializeField] protected string prompt;
    [SerializeField] protected TextMeshProUGUI interactionTextUI;
    [SerializeField] protected GameObject[] interactionIndicators;
    [SerializeField] protected float countdownTime;
    [SerializeField] public Slider timeCountUi;

    protected virtual void Start()
    {
        interactionTextUI.text = prompt;
    }
    
    protected virtual void Update()
    {
        
    }
    
    public virtual void OnTarget(bool isTarget)
    {
        foreach (GameObject interactionIndicator in interactionIndicators)
            interactionIndicator.SetActive(isTarget);
        
        InteractHandler();
    }

    public virtual void InteractHandler()
    {
        // Pls override this method
        // Do something when interact
    }

    protected IEnumerator CountdownAndDestroy(float time)
    {
        float timeCount = 0;
        
        while (timeCount < time)
        {
            if (timeCount/time <= 0.85f)
            {
                if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
                {
                    timeCountUi.gameObject.SetActive(false);
                    yield break;
                } 
            }

            timeCountUi.gameObject.SetActive(true);

            float progress = timeCount / time;
            timeCountUi.value = Mathf.Lerp(1f, 0f, progress);
            
            timeCount += Time.deltaTime;
            yield return null;
        }
        GameManager.instance.AddPoint(point);
        Destroy(gameObject);
    }
}
