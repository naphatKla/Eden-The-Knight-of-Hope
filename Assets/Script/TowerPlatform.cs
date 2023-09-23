using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class TowerPlatform : InteractableObject
{
    [SerializeField] private GameObject towerPrefab;
    [SerializeField] private int cost;
    private GameObject _tower;

    public override void Interact()
    {
        base.Interact();
        
        if(!Input.GetKeyDown(key)) return;
        if(GameManager.instance.totalPoint < cost) return;
        
        if (_tower != null)
        {
            HealthSystem towerHealth = _tower.GetComponent<HealthSystem>();
            if(towerHealth._currentHp >= towerHealth.maxHp) return;
            towerHealth.FullHeal();
            GameManager.instance.AddPoint(-1*cost);
            return;
        }
        
        _tower = Instantiate(towerPrefab,transform.position+new Vector3(0,-0.5f,0),Quaternion.identity,transform);
        GameManager.instance.AddPoint(-1*cost);
    }
}
