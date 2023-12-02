using System;
using System.Collections.Generic;
using System.Linq;
using Inventory;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TowerBuilderUIPage : MonoBehaviour
{
    [Header("TowerZone")]
    [SerializeField] private Transform towerContent;
    [SerializeField] private TowerUIItem towerUIItemPrefab;
    private List<TowerUIItem> _towerUIItems = new List<TowerUIItem>();
    
    [Header("DetailsZone")]
    [SerializeField] private Image towerImage;
    [SerializeField] private TextMeshProUGUI towerName;
    [SerializeField] private TextMeshProUGUI towerDescription;
    [SerializeField] private TextMeshProUGUI towerCost;
    [SerializeField] private TextMeshProUGUI totalCoin;
    
    [Header("RequirementZone")]
    [SerializeField] private Transform requirementContent;
    [SerializeField] private RequireUIItem requirementItemPrefab;
    private List<RequireUIItem> _requireUIItems = new List<RequireUIItem>();
    [HideInInspector] public TowerSO currentTowerItem; //
    public Action OnBuildTower;

    [SerializeField] private Button buildButton;
    [SerializeField] private Button closeButton;
    [SerializeField] private List<TowerSO> towerRecipes;

    private void Start()
    {
        closeButton.onClick.AddListener(() => gameObject.transform.parent.gameObject.SetActive(false));
        Initialize();
    }

    public void Initialize()
    {
        buildButton.onClick.AddListener(BuildTower);
        for (int i = 0; i < towerContent.childCount; i++)
        {
            if (towerContent.GetChild(i).GetComponent<TowerUIItem>() == null) continue;
            Destroy(towerContent.GetChild(i).gameObject);
        }
        
        foreach (TowerSO tower in towerRecipes)
        {
            TowerUIItem towerUIItem = Instantiate(towerUIItemPrefab, towerContent);
            towerUIItem.SetData(tower);
            towerUIItem.GetComponent<Button>().onClick.AddListener(() => SetDescriptionAndRequirementData(tower));
            _towerUIItems.Add(towerUIItem);
        }
        
        for(int i = 0; i < requirementContent.childCount; i++) Destroy(requirementContent.GetChild(i).gameObject);
        for (int i = 0; i < 10; i++)
        {
            RequireUIItem item = Instantiate(requirementItemPrefab, requirementContent);
            item.gameObject.SetActive(false);
            _requireUIItems.Add(item);
        }
        currentTowerItem = towerRecipes[0];
        SetDescriptionAndRequirementData(currentTowerItem);
    }

    private void SetDescriptionAndRequirementData(TowerSO towerSo)
    {
        towerImage.sprite = towerSo.towerImage;
        towerName.text = towerSo.towerName;
        towerDescription.text = towerSo.towerDescription;
        towerCost.text = GameManager.Instance.totalPoint >= towerSo.cost? $"<color=#05B900> Cost: ${towerSo.cost}": $"<color=red> Cost: ${towerSo.cost}";
        totalCoin.text = $"{GameManager.Instance.totalPoint}";

        List<InventoryItem> requirementItems = towerSo.requireItems.ToList();

        for(int i = 0; i < _requireUIItems.Count; i++)
        {
            if (i >= requirementItems.Count)
            {
                _requireUIItems[i].gameObject.SetActive(false);
                continue;
            }
            _requireUIItems[i].gameObject.SetActive(true);
            int available = PlayerInventoryController.Instance.InventoryData.GetAllQuantityOfItem(requirementItems[i]);
            _requireUIItems[i].SetData(requirementItems[i], requirementItems[i].quantity, available);
            currentTowerItem = towerSo;
        }
    }

    public void UpdatePage()
    {
        _towerUIItems.ForEach(tower => tower.SetCorrectIcon(tower.TowerRecipe.CheckRecipe() && GameManager.Instance.totalPoint >= tower.TowerRecipe.cost));
        if(currentTowerItem) 
            SetDescriptionAndRequirementData(currentTowerItem);
    }

    private void BuildTower()
    {
        InventorySo inventoryData = PlayerInventoryController.Instance.InventoryData;
        if (!currentTowerItem.CheckRecipe()) return;
        OnBuildTower?.Invoke();
        foreach (InventoryItem requireItem in currentTowerItem.requireItems)
        {
            inventoryData.RemoveItem(requireItem.item, requireItem.quantity);
        }
        UpdatePage();
    }
}
