using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerCamera : MonoBehaviour
{
    //CameraFollowPlayer
    [SerializeField] private Transform player;
    [Space] [SerializeField] private float smoothTime = 0.25f;
    private Vector3 offset = new Vector3(0f, 0f, -10f);
    private Vector3 _velocity = Vector3.zero;
    
    //Zoom
    [SerializeField] private float zoomSmoothTime = 0.25f;
    [SerializeField] private float zoomMultiplier = 4f;
    [SerializeField] private float minZoom = 2f;
    [SerializeField] private float maxZoom = 8f;
    private float _velocity2;
    private float _zoom;
    private Camera _cam;

    void Start()
    {
        _cam = GetComponent<Camera>();
        _zoom = _cam.orthographicSize;
    }
    void Update()
    {
        //CameraFollowPlayer
        Vector3 targetPosition = player.position + offset;
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref _velocity, smoothTime);
        
        //Zoom
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        _zoom -= scroll * zoomMultiplier;
        _zoom = Mathf.Clamp(_zoom, minZoom, maxZoom);
        _cam.orthographicSize = Mathf.SmoothDamp(_cam.orthographicSize, _zoom, ref _velocity2, zoomSmoothTime);
    }
}
