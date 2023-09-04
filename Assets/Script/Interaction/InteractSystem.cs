using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InteractSystem : MonoBehaviour
{
    [SerializeField] private Transform interactionPoint;
    [SerializeField] private float interactionPointRadius = 0.5f ;
    [SerializeField] private LayerMask interactableMask;
    
    void Start()
    {
        
    }
    
    private void Update()
    {
        InteracHandle();
    }

    private void InteracHandle()
    {
        Collider2D[] collider2Ds =
            Physics2D.OverlapCircleAll(interactionPoint.position, interactionPointRadius, interactableMask);

        if (collider2Ds.Length <= 0) return;

        List<float> distances = new List<float>();
        InteractableObject interactableObject;

        foreach (Collider2D col in collider2Ds)
            distances.Add(Vector2.Distance(transform.position, col.transform.position));

        // interact only the nearest object
        interactableObject = collider2Ds[distances.IndexOf(distances.Min())].gameObject.GetComponent<InteractableObject>();
        interactableObject.Interact();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(interactionPoint.position, interactionPointRadius);
    }
    




}
