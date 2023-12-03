using System.Collections.Generic;
using Inventory;
using UnityEngine;
using UnityEngine.UI;

namespace Interaction
{
    public class TowerPlatform : InteractableObject
    {
        [SerializeField] private Image towerUpgradeUI;
        [SerializeField] private TowerBuilderUIPage towerBuilderUIPage;
        private GameObject _towerOnPlatform;
        private TowerSO _currentTowerSO;
        private float _lastOpenTime;
        private bool _isCloseUI;
        private int _maxTierBuilded = 0;
        [SerializeField] private LayerMask playerLayer;
        
        protected void Update()
        {
            if (_towerOnPlatform && Input.GetKeyDown(KeyCode.C))
                _towerOnPlatform.GetComponent<HealthSystem.HealthSystem>().TakeDamage(50);
                
            if (!_towerOnPlatform)
            {
                if (_currentTowerSO)
                {
                    _currentTowerSO = null;
                    float towerHpPercentage = _towerOnPlatform ? (_towerOnPlatform.GetComponent<HealthSystem.HealthSystem>().CurrentHp / _towerOnPlatform.GetComponent<HealthSystem.HealthSystem>().maxHp) * 100 : 0;
                    towerBuilderUIPage.UpdatePage(_maxTierBuilded, _currentTowerSO, towerHpPercentage);
                }
            }
               
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
            float towerHpPercentage = _towerOnPlatform ? (_towerOnPlatform.GetComponent<HealthSystem.HealthSystem>().CurrentHp / _towerOnPlatform.GetComponent<HealthSystem.HealthSystem>().maxHp) * 100 : 0;
            towerUpgradeUI.gameObject.SetActive(true);
            towerBuilderUIPage.UpdatePage(_maxTierBuilded, _currentTowerSO, towerHpPercentage);
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
            InventorySo inventoryData = PlayerInventoryController.Instance.InventoryData;
            TowerSO currentTowerItemOnPage = towerBuilderUIPage.currentTowerItemOnPage;
            BuildTowerState buildTowerState = towerBuilderUIPage.buildTowerState;
            float towerHpPercentage = _towerOnPlatform ? (_towerOnPlatform.GetComponent<HealthSystem.HealthSystem>().CurrentHp / _towerOnPlatform.GetComponent<HealthSystem.HealthSystem>().maxHp) * 100 : 0;
            towerBuilderUIPage.UpdatePage(_maxTierBuilded, _currentTowerSO,towerHpPercentage);
            
            if (buildTowerState == BuildTowerState.Build)
            {
                if (_towerOnPlatform) return;
                if (!currentTowerItemOnPage.CheckRecipe()) return;
                int cost = currentTowerItemOnPage.cost;
                if (GameManager.Instance.totalPoint < cost) return;
                
                Tower.Tower towerPrefab = currentTowerItemOnPage.towerPrefab;
                foreach (InventoryItem requireItem in currentTowerItemOnPage.requireItems)
                {
                    inventoryData.RemoveItem(requireItem.item, requireItem.quantity);
                }
                GameManager.Instance.AddPoint(-cost);
                
                _towerOnPlatform = Instantiate(towerPrefab.gameObject, transform.position + new Vector3(0, -0.28f, 0), Quaternion.identity, transform);
                _currentTowerSO = currentTowerItemOnPage;
                _maxTierBuilded = _currentTowerSO.tier > _maxTierBuilded ? _currentTowerSO.tier : _maxTierBuilded;
                towerHpPercentage = _towerOnPlatform ? (_towerOnPlatform.GetComponent<HealthSystem.HealthSystem>().CurrentHp / _towerOnPlatform.GetComponent<HealthSystem.HealthSystem>().maxHp) * 100 : 0;
                towerBuilderUIPage.UpdatePage(_maxTierBuilded, _currentTowerSO, towerHpPercentage);
            }
            else if (buildTowerState == BuildTowerState.Repair)
            {
                if (!currentTowerItemOnPage.CheckRepairRecipe(towerHpPercentage)) return;
                int cost = currentTowerItemOnPage.GetRepairState(towerHpPercentage).repairCost;
                if (GameManager.Instance.totalPoint < cost) return;
                
                foreach (InventoryItem repairItem in currentTowerItemOnPage.GetRepairState(towerHpPercentage).repairItems)
                {
                    inventoryData.RemoveItem(repairItem.item, repairItem.quantity);
                }
                GameManager.Instance.AddPoint(-cost);
                _towerOnPlatform.GetComponent<HealthSystem.HealthSystem>().ResetHealth();
                towerHpPercentage = _towerOnPlatform ? (_towerOnPlatform.GetComponent<HealthSystem.HealthSystem>().CurrentHp / _towerOnPlatform.GetComponent<HealthSystem.HealthSystem>().maxHp) * 100 : 0;
                towerBuilderUIPage.UpdatePage(_maxTierBuilded, _currentTowerSO, towerHpPercentage);
            }
            else if (buildTowerState == BuildTowerState.Upgrade)
            {
                if (!currentTowerItemOnPage.CheckRecipe()) return;
                int cost = currentTowerItemOnPage.cost;
                if (GameManager.Instance.totalPoint < currentTowerItemOnPage.cost) return;
                
                Tower.Tower towerPrefab = currentTowerItemOnPage.towerPrefab;
                foreach (InventoryItem requireItem in currentTowerItemOnPage.requireItems)
                {
                    inventoryData.RemoveItem(requireItem.item, requireItem.quantity);
                }
                GameManager.Instance.AddPoint(-cost);
                
                Destroy(_towerOnPlatform);
                _towerOnPlatform = Instantiate(towerPrefab.gameObject, transform.position + new Vector3(0, -0.28f, 0), Quaternion.identity, transform);
                _currentTowerSO = currentTowerItemOnPage;
                _maxTierBuilded = _currentTowerSO.tier > _maxTierBuilded ? _currentTowerSO.tier : _maxTierBuilded;
                towerHpPercentage = _towerOnPlatform ? (_towerOnPlatform.GetComponent<HealthSystem.HealthSystem>().CurrentHp / _towerOnPlatform.GetComponent<HealthSystem.HealthSystem>().maxHp) * 100 : 0;
                towerBuilderUIPage.UpdatePage(_maxTierBuilded, _currentTowerSO,towerHpPercentage);
            }
        }
    }
}
