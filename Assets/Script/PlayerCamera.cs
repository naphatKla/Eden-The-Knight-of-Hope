using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    //CameraFollowPlayer
    [SerializeField] private Transform Player;
    private Vector3 offset = new Vector3(0f, 0f, -10f);
    private float smoothTime = 0.25f;
    private Vector3 velocity = Vector3.zero;
    
    //Zoom
    [SerializeField] private Camera cam;
    private float zoom;
    private float zoomMultiplier = 4f;
    private float minZoom = 2f;
    private float maxZoom = 8f;
    private float velocity2 = 0f;
    private float zoomSmoothTime = 0.25f;

    void Start()
    {
        //Zoom
        zoom = cam.orthographicSize;
    }
    void Update()
    {
        //CameraFollowPlayer
        Vector3 targetPosition = Player.position + offset;
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
        //Zoom
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        zoom -= scroll * zoomMultiplier;
        zoom = Mathf.Clamp(zoom, minZoom, maxZoom);
        cam.orthographicSize = Mathf.SmoothDamp(cam.orthographicSize, zoom, ref velocity2, zoomSmoothTime);
    }
}
