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
    [SerializeField] protected string prompt;
    [SerializeField] protected TextMeshProUGUI interactionTextUI;
    [SerializeField] protected GameObject[] interactionIndicators;
    [SerializeField] protected int point;
    
    //countdownTime 
    [SerializeField] protected float countdownTime;

    // UI Bar TimeCount
    [SerializeField] public Slider timeCountUi;

    protected virtual void Start()
    {
        interactionTextUI.text = prompt;
    }
    
    protected virtual void Update()
    {
        
    }
    
    public virtual void Interact()
    {
        StartCoroutine(TriggerIndicators());
        if (!Input.GetKeyDown(key)) return;
        // Do something when player interact
    }

    protected IEnumerator CountdownAndDestroy(float time)
    {
        float timeCount = 0;
        
        while (timeCount < time)
        {
            if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
            {
                timeCountUi.gameObject.SetActive(false);
                yield break;
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

    IEnumerator TriggerIndicators()
    {
        foreach (GameObject interactionIndicator in interactionIndicators)
            interactionIndicator.SetActive(true);

        yield return new WaitForEndOfFrame();
        foreach (GameObject interactionIndicator in interactionIndicators)
            interactionIndicator.SetActive(false);
    }
    
}
