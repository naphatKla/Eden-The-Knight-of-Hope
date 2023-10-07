using UnityEngine;

namespace Interaction
{
    public class TowerPlatform : InteractableObject
    {
        [SerializeField] private GameObject towerPrefab;
        [SerializeField] private int cost;
        private HealthSystem.HealthSystem _towerHealthSystem;
        private GameObject _tower;

        protected override void InteractHandler()
        {
            if(!Input.GetKeyDown(key)) return;
            if(GameManager.instance.totalPoint < cost) return;
            base.InteractHandler();
        }
        
        protected override void InteractAction()
        {
            if (_tower)
            {
                if (_towerHealthSystem.CurrentHp >= _towerHealthSystem.maxHp) return;
                _towerHealthSystem.ResetHealth();
                GameManager.instance.AddPoint(-1*cost);
                return;
            }
        
            Transform thisTransform = transform;
            _tower = Instantiate(towerPrefab,thisTransform.position + new Vector3(0,-0.5f,0),Quaternion.identity,thisTransform);
            _towerHealthSystem = _tower.GetComponent<HealthSystem.HealthSystem>();
            GameManager.instance.AddPoint(-1*cost);
        }
    }
}
