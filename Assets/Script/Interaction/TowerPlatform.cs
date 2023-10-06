using System.Collections;
using UnityEngine;

public class TowerPlatform : InteractableObject
{
    [SerializeField] private GameObject towerPrefab;
    [SerializeField] private int cost;
    private GameObject _tower;
    
    public override void InteractHandler()
    {
        if(!Input.GetKeyDown(key)) return;
        if(GameManager.instance.totalPoint < cost) return;
        StartCoroutine(BuildTower(countdownTime));
    }
    
    private IEnumerator BuildTower(float duration)
    {
        float timeCount = 0;
        
        while (timeCount < duration)
        {
            if (timeCount/duration <= 0.85f)
            {
                if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
                {
                    timeCountUi.gameObject.SetActive(false);
                    yield break;
                } 
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

            if (towerHealth.CurrentHp < towerHealth.maxHp)
            {
                towerHealth.ResetHealth();
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
