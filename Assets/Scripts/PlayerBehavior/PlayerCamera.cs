using UnityEngine;

namespace PlayerBehavior
{
    public class PlayerCamera : MonoBehaviour
    {
        #region Declare Varaibles
        [Header("CameraFollowPlayer")]
        [SerializeField] private Transform player;
        [Space] [SerializeField] private float smoothTime;
        [SerializeField] private Vector3 offset;
        private Vector3 _velocity = Vector3.zero;

        [Header("Zoom")] 
        [SerializeField] private float zoomSmoothTime;
        [SerializeField] private float zoomMultiplier;
        [SerializeField] private float minZoom;
        [SerializeField] private float maxZoom;
        private float _velocity2;
        private float _zoom;
        private Camera _cam;
        #endregion
        
        private void Start()
        {
            _cam = GetComponent<Camera>();
            _zoom = (minZoom + maxZoom) / 2;
        }
        private void Update()
        {
            //CameraFollowPlayer
            if(!player) return;
            Vector3 targetPosition = player.position + offset;
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref _velocity, smoothTime);
        
            if (UIManager.Instance.isAnyUIOpen) return;
            //Zoom
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            _zoom -= scroll * zoomMultiplier;
            _zoom = Mathf.Clamp(_zoom, minZoom, maxZoom);
            _cam.orthographicSize = Mathf.SmoothDamp(_cam.orthographicSize, _zoom, ref _velocity2, zoomSmoothTime);
        }
    }
}
