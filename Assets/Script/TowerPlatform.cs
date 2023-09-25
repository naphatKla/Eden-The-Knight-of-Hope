using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class TowerPlatform : InteractableObject
{
    [SerializeField] private GameObject towerPrefab;
    [SerializeField] private int cost;
    private GameObject _tower;

    protected override void Start()
    {
        base.Start();
    }

    public override void Interact()
    {
        base.Interact();
        
        if(!Input.GetKeyDown(key)) return;
        if(GameManager.instance.totalPoint < cost) return;
        StartCoroutine(BuildTower(countdownTime));

    }

    private IEnumerator BuildTower(float duration)
    {
        float timeCount = 0;
        
        while (timeCount < duration)
        {
            if (Mathf.Abs(Input.GetAxis("Horizontal")) > 0.5f || Mathf.Abs(Input.GetAxis("Vertical")) > 0.5f )
            {
                timeCountUi.gameObject.SetActive(false);
                yield break;
            }
            timeCountUi.gameObject.SetActive(true);

            float progress = timeCount / duration;
            timeCountUi.value = Mathf.Lerp(1f, 0f, progress);
            
            timeCount += Time.deltaTime;
            yield return null;
        }

        if (_tower != null)
        {
            HealthSystem towerHealth = _tower.GetComponent<HealthSystem>();

            if (towerHealth._currentHp < towerHealth.maxHp)
            {
                towerHealth.FullHeal();
                GameManager.instance.AddPoint(-1*cost);
            }
        }
        else
        {
            _tower = Instantiate(towerPrefab,transform.position+new Vector3(0,-0.5f,0),Quaternion.identity,transform);
            GameManager.instance.AddPoint(-1*cost);
        }
        
        timeCountUi.gameObject.SetActive(false);
    }
}
