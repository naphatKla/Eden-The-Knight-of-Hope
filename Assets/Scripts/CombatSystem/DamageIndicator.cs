using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageIndicator : MonoBehaviour
{
    private float destroyTime = 1f;
    private Vector3 Offset = new Vector3(0, 3, 0);
    void Start()
    {
        Destroy(gameObject,destroyTime);
        transform.localPosition += Offset;

    }
    
}
