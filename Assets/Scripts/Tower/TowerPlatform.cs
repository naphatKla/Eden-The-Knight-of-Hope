using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Interaction
{
    public class TowerPlatform : InteractableObject
    {
        [SerializeField] private Image towerUpgradeUI;
        [SerializeField] private TowerBuilderUIPage towerBuilderUIPage;
        private HealthSystem.HealthSystem _towerHealthSystem;
        private GameObject _tower;
        private TowerSO _currentTowerSO;
        private float _lastOpenTime;
        private bool _isCloseUI;
        [SerializeField] private LayerMask playerLayer;
        
        protected void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
                CloseTowerUI();
            if(_lastOpenTime + 1.5f > Time.time) return;
            if (Physics2D.OverlapBoxNonAlloc(transform.position, new Vector2(10, 10), 0, new Collider2D[1],
                    playerLayer) == 0)
            {
                if (!_isCloseUI)
                {
                    CloseTowerUI();
                    _isCloseUI = true;
                }
            }
        }
        
        private void OpenTowerUI()
        {
            if (UIManager.Instance.CheckIsAnyUIOpen()) return;
            towerUpgradeUI.gameObject.SetActive(true);
            towerBuilderUIPage.UpdatePage();
            _lastOpenTime = Time.time;
            _isCloseUI = false;

            towerBuilderUIPage.OnBuildTower = null;
            towerBuilderUIPage.OnBuildTower += BuildTower;
        }

        private void CloseTowerUI()
        {
            towerUpgradeUI.gameObject.SetActive(false);
        }
        
        /// <summary>
        /// Create tower if the tower is not exist, else reset the tower health.
        /// </summary>
        protected override void InteractAction()
        {
            OpenTowerUI();
        }

        private void BuildTower()
        {
            if (_tower) return;
            Tower.Tower towerPrefab = towerBuilderUIPage.currentTowerItem.towerPrefab;
            int cost = towerBuilderUIPage.currentTowerItem.cost;
            Transform thisTransform = transform;
            _tower = Instantiate(towerPrefab.gameObject, thisTransform.position + new Vector3(0, -0.28f, 0), Quaternion.identity,
                thisTransform);
            _currentTowerSO = towerBuilderUIPage.currentTowerItem;
            _towerHealthSystem = _tower.GetComponent<HealthSystem.HealthSystem>();
            GameManager.Instance.AddPoint(-cost);
        }
    }
}
