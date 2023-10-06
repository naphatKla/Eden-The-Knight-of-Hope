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
    private InteractableObject _targetObject;
    private InteractableObject _lastTargetObject;
    #endregion
    private void Update()
    {
        _objectsInArea = Physics2D.OverlapCircleAll(interactionPoint.position, interactionPointRadius, interactableMask).ToList();
        if (_objectsInArea.Count <= 0)
        {
            _targetObject?.OnTarget(false);
            _targetObject = null;
            return;
        }
        
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
        // Target only the nearest object
        _targetObject = _objectsInArea[objectDistances.IndexOf(objectDistances.Min())].gameObject.GetComponent<InteractableObject>();
        _targetObject.OnTarget(true);

        if (_lastTargetObject != _targetObject)
            _lastTargetObject?.OnTarget(false);
        
        _lastTargetObject = _targetObject;
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
