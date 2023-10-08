using UnityEngine;

namespace Interaction
{
    public class TowerPlatform : InteractableObject
    {
        [SerializeField] private GameObject towerPrefab;
        [SerializeField] private int cost;
        private HealthSystem.HealthSystem _towerHealthSystem;
        private GameObject _tower;

        /// <summary>
        /// If point is not enough, do not interact.
        /// </summary>
        protected override void InteractHandler()
        {
            if(GameManager.Instance.totalPoint < cost) return;
            base.InteractHandler();
        }
        
        /// <summary>
        /// Create tower if the tower is not exist, else reset the tower health.
        /// </summary>
        protected override void InteractAction()
        {
            if (_tower)
            {
                if (_towerHealthSystem.CurrentHp >= _towerHealthSystem.maxHp) return;
                _towerHealthSystem.ResetHealth();
                GameManager.Instance.AddPoint(-cost);
                return;
            }
        
            Transform thisTransform = transform;
            _tower = Instantiate(towerPrefab,thisTransform.position + new Vector3(0,-0.5f,0),Quaternion.identity,thisTransform);
            _towerHealthSystem = _tower.GetComponent<HealthSystem.HealthSystem>();
            GameManager.Instance.AddPoint(-cost);
        }
    }
}
