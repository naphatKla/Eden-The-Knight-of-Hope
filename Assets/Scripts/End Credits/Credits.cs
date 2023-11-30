using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Credits : MonoBehaviour
{
    [SerializeField] private float scrollSpeed;
    
    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.up * scrollSpeed * Time.deltaTime);
    }
}
