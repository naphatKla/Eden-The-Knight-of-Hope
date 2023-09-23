using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GatheringResource : InteractableObject
{

    public override void Interact()
    {
        base.Interact();
        
        if(!Input.GetKeyDown(key)) return;
        StartCoroutine(CountdownAndDestroy(countdownTime));
    }
}
