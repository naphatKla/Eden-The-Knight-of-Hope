using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

public class InteractableObject : MonoBehaviour
{
    [SerializeField] private KeyCode key;
    [SerializeField] private string prompt;
    [SerializeField] private TextMeshProUGUI interactionTextUI;
    [SerializeField] private GameObject[] interactionIndicators;
    [SerializeField] private int point;
    
    //countdownTime 
    [SerializeField] private float countdownTime;
    private bool canCollect = true;

    void Start()
    {
        interactionTextUI.text = prompt;
    }
    
    void Update()
    {
       
    }
    
    public virtual void Interact()
    {
        StartCoroutine(TriggerIndicators());
        if (!Input.GetKeyDown(key)) return;
        // Do something when player interact
        
        // Reward Point & Destroy
        StartCoroutine(CountdownAndDestroy(countdownTime));
        
    }
    
    IEnumerator CountdownAndDestroy(float time)
    {
        float timeCount = 0;
        
        while (timeCount < time)
        {
            if(Input.GetKeyUp(key)) yield break;
            Debug.Log($"{timeCount:F1}");
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
