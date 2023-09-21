using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

public class InteractableObject : MonoBehaviour
{
    [SerializeField] protected KeyCode key;
    [SerializeField] protected string prompt;
    [SerializeField] protected TextMeshProUGUI interactionTextUI;
    [SerializeField] protected GameObject[] interactionIndicators;

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
        if(!Input.GetKeyDown(key)) return;
        // Do something when player interact
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
