using System.Collections.Generic;
using System.Linq;
using Interaction;
using UnityEngine;
using UnityEngine.Serialization;

namespace PlayerBehavior
{
    public class PlayerInteractSystem : MonoBehaviour
    {
        #region Declare Variables
        [SerializeField] private Transform interactionPoint;
        [SerializeField] private float interactionPointRadius;
        [SerializeField] private LayerMask interactableMask;
        private readonly Collider2D[] _objectsInArea = new Collider2D[1];
        private InteractableObject _targetObject;
        private InteractableObject _lastTargetObject;
        public bool isStopMove;
        public static PlayerInteractSystem Instance;
        #endregion
        
        private void Awake()
        {
            Instance = this;
        }
        private void Update()
        {
            float objCount = Physics2D.OverlapCircleNonAlloc(interactionPoint.position, interactionPointRadius, _objectsInArea, interactableMask);
        
            if (objCount <= 0)
            {
                if (!_targetObject) return;
                _targetObject.OnTarget(false);
                _targetObject = null;
                return;
            }
        
            InteractHandle();
        }

        #region Methods
        // ReSharper disable Unity.PerformanceAnalysis
        /// <summary>
        /// Target the nearest object in the interaction radius.
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

            if (_lastTargetObject != _targetObject && _lastTargetObject)
                _lastTargetObject.OnTarget(false);
        
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
}

