using UnityEngine;

namespace Interaction
{
    public class TowerPlatform : InteractableObject
    {
        [SerializeField] private GameObject towerPrefab;
        [SerializeField] private int cost;
        private HealthSystem.HealthSystem _towerHealthSystem;
        private GameObject _tower;

        protected override void Start()
        {
            base.Start();
            interactionTextUI.text = $"Press E to build the tower.\n<color=blue>Cost: {cost} coins</color>";
        }

        /// <summary>
        /// If point is not enough, do not interact.
        /// </summary>
        protected override void InteractHandler()
        {
            if(GameManager.Instance.totalPoint < cost) return;
            if (_tower)
            {
                interactionTextUI.text = $"Press E to repair the tower.\n<color=blue>Cost: {cost/2} coins</color>";
                if (_towerHealthSystem.CurrentHp >= _towerHealthSystem.maxHp) return;
            }
            base.InteractHandler();
        }
        
        /// <summary>
        /// Create tower if the tower is not exist, else reset the tower health.
        /// </summary>
        protected override void InteractAction()
        {
            if (_tower)
            {
                _towerHealthSystem.ResetHealth();
                GameManager.Instance.AddPoint(-cost/2);
                return;
            }
        
            Transform thisTransform = transform;
            _tower = Instantiate(towerPrefab,thisTransform.position + new Vector3(0,-0.5f,0),Quaternion.identity,thisTransform);
            _towerHealthSystem = _tower.GetComponent<HealthSystem.HealthSystem>();
            GameManager.Instance.AddPoint(-cost);
        }
    }
}
