using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class TowerPlatform : InteractableObject
{
    [SerializeField] private GameObject towerPrefab;
    private GameObject _tower;

    public override void Interact()
    {
        base.Interact();
        
        if(!Input.GetKeyDown(key)) return;
        if (_tower != null) return;
        _tower = Instantiate(towerPrefab,transform.position+new Vector3(0,-0.5f,0),Quaternion.identity,transform);
    }
}
