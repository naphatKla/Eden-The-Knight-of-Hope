using System.Collections;
using System.Collections.Generic;
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
