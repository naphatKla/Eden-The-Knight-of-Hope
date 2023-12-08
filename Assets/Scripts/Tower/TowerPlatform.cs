using Inventory;
using UnityEngine;
using UnityEngine.UI;

namespace Interaction
{
    public class TowerPlatform : InteractableObject
    {
        [SerializeField] private Image towerUpgradeUI;
        [SerializeField] private TowerBuilderUIPage towerBuilderUIPage;
        [HideInInspector] public GameObject towerOnPlatform;
        private TowerSO _currentTowerSO;
        private float _lastOpenTime;
        private bool _isCloseUI;
        private int _maxTierBuilded = 0;
        private TowerHealthSystem _towerHealthSystem;
        [SerializeField] private LayerMask playerLayer;
        [Header("Sound")] [SerializeField] private AudioClip[] openTowerPlatformSounds;
        [SerializeField] private AudioClip[] closeTowerPlatformSounds;
        
        protected override void Update()
        {
            if (!towerOnPlatform)
            {
                if (_currentTowerSO)
                {
                    _currentTowerSO = null;
                    float towerHpPercentage = _towerHealthSystem ? (_towerHealthSystem.CurrentHp / _towerHealthSystem.maxHp) * 100 : 0;
                    towerBuilderUIPage.UpdatePage(_maxTierBuilded, _currentTowerSO, towerHpPercentage);
                }
            }
               
            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Tab))
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
            towerBuilderUIPage.TowerPlatformLinked = this;
            float towerHpPercentage = towerOnPlatform ? (_towerHealthSystem.CurrentHp / _towerHealthSystem.maxHp) * 100 : 0;
            towerBuilderUIPage.currentTowerItemOnPage = _maxTierBuilded <= 0? towerBuilderUIPage.currentTowerItemOnPage : _currentTowerSO;
            towerBuilderUIPage.UpdatePage(_maxTierBuilded, _currentTowerSO, towerHpPercentage);
            _lastOpenTime = Time.time;
            _isCloseUI = false;

            towerBuilderUIPage.OnBuildTower = null;
            towerBuilderUIPage.OnBuildTower += BuildTower;
            SoundManager.Instance.RandomPlaySound(openTowerPlatformSounds);
        }
        
        private void UpdateTowerUIPage()
        {
            if (towerUpgradeUI.gameObject.activeSelf == false) return;
            if (towerBuilderUIPage.TowerPlatformLinked != this) return;
            float towerHpPercentage = towerOnPlatform ? (_towerHealthSystem.CurrentHp / _towerHealthSystem.maxHp) * 100 : 0;
            towerBuilderUIPage.UpdatePage(_maxTierBuilded, _currentTowerSO, towerHpPercentage);
        }

        public void CloseTowerUI()
        {
            if (towerUpgradeUI.gameObject.activeSelf == false) return;
            towerUpgradeUI.gameObject.SetActive(false);
            SoundManager.Instance.RandomPlaySound(closeTowerPlatformSounds);
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
            float towerHpPercentage = _towerHealthSystem ? (_towerHealthSystem.CurrentHp / _towerHealthSystem.maxHp) * 100 : 0;
            towerBuilderUIPage.UpdatePage(_maxTierBuilded, _currentTowerSO,towerHpPercentage);
            
            if (buildTowerState == BuildTowerState.Build)
            {
                if (towerOnPlatform) return;
                if (!currentTowerItemOnPage.CheckRecipe()) return;
                int cost = currentTowerItemOnPage.cost;
                if (GameManager.Instance.totalPoint < cost) return;
                
                Tower.Tower towerPrefab = currentTowerItemOnPage.towerPrefab;
                foreach (InventoryItem requireItem in currentTowerItemOnPage.requireItems)
                {
                    inventoryData.RemoveItem(requireItem.item, requireItem.quantity);
                }
                GameManager.Instance.AddPoint(-cost);
                
                towerOnPlatform = Instantiate(towerPrefab.gameObject, transform.position + new Vector3(0, -0.28f, 0), Quaternion.identity, transform);
                _currentTowerSO = currentTowerItemOnPage;
                _maxTierBuilded = _currentTowerSO.tier > _maxTierBuilded ? _currentTowerSO.tier : _maxTierBuilded;
                _towerHealthSystem = towerOnPlatform?.GetComponent<TowerHealthSystem>();
                if (_towerHealthSystem)
                    _towerHealthSystem.OnTowerDamaged += UpdateTowerUIPage;
                towerHpPercentage = towerOnPlatform? (_towerHealthSystem.CurrentHp / _towerHealthSystem.maxHp) * 100 : 0;
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
                _towerHealthSystem = towerOnPlatform?.GetComponent<TowerHealthSystem>();
                _towerHealthSystem.ResetHealth();
                towerHpPercentage = towerOnPlatform ? (_towerHealthSystem.CurrentHp / _towerHealthSystem.maxHp) * 100 : 0;
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
                
                Destroy(towerOnPlatform);
                towerOnPlatform = Instantiate(towerPrefab.gameObject, transform.position + new Vector3(0, -0.28f, 0), Quaternion.identity, transform);
                _currentTowerSO = currentTowerItemOnPage;
                _maxTierBuilded = _currentTowerSO.tier > _maxTierBuilded ? _currentTowerSO.tier : _maxTierBuilded;
                _towerHealthSystem = towerOnPlatform?.GetComponent<TowerHealthSystem>();
                if (_towerHealthSystem)
                    _towerHealthSystem.OnTowerDamaged += UpdateTowerUIPage;
                towerHpPercentage = towerOnPlatform ? (_towerHealthSystem.CurrentHp / _towerHealthSystem.maxHp) * 100 : 0;
                towerBuilderUIPage.UpdatePage(_maxTierBuilded, _currentTowerSO,towerHpPercentage);
            }
        }
    }
}
