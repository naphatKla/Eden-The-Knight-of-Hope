using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerInteractSystem : MonoBehaviour
{
    #region Declare Variables
    [SerializeField] private Transform interactionPoint;
    [SerializeField] private float interactionPointRadius;
    [SerializeField] private LayerMask interactableMask;
    private List<Collider2D> _objectsInArea;
    private InteractableObject interactableObject;
    #endregion
    private void Update()
    {
        _objectsInArea = Physics2D.OverlapCircleAll(interactionPoint.position, interactionPointRadius, interactableMask).ToList();
        if (_objectsInArea.Count <= 0) return;
        
        InteractHandle();
    }

    #region Methods
    // ReSharper disable Unity.PerformanceAnalysis
    /// <summary>
    /// Interact with the nearest object.
    /// </summary>
    private void InteractHandle()
    {
        List<float> objectDistances = new List<float>();

        foreach (Collider2D obj in _objectsInArea)
        {
            float objectDistance = Vector2.Distance(transform.position, obj.transform.position);
            objectDistances.Add(objectDistance);
        }
        
        // interact only the nearest object
        interactableObject = _objectsInArea[objectDistances.IndexOf(objectDistances.Min())].gameObject.GetComponent<InteractableObject>();
        interactableObject.Interact();
    }

    
    /// <summary>
    /// draw interaction radius in the inspector.
    /// </summary>
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(interactionPoint.position, interactionPointRadius);
    }
    #endregion
}
