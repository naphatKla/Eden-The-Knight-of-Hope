using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    void Start()
    {
        
    }
    
    void Update()
    {
        
    }

    public virtual void Interact()
    {
        Debug.Log($"Interact!! {gameObject.name}");
    }
}
