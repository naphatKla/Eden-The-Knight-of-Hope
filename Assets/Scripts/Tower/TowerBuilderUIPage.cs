using System;
using System.Collections.Generic;
using System.Linq;
using Inventory;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public enum BuildTowerState
{
    Build,
    Upgrade,
    Repair
}
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
    [FormerlySerializedAs("currentTowerItem")] [HideInInspector] public TowerSO currentTowerItemOnPage; //
    public Action OnBuildTower;

    [SerializeField] private Button buildButton;
    [SerializeField] private TextMeshProUGUI buildButtonText;
    [SerializeField] private Button closeButton;
    [SerializeField] private List<TowerSO> towerRecipes;
    public BuildTowerState buildTowerState;
    private int lastedMaxTierBuilded = 0;
    private TowerSO lastedCurrentTowerSoOnPlatform;
    private float lastedCurrentTowerHpPercentage = 100;

    private void Awake()
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
            towerUIItem.GetComponent<Button>().onClick.AddListener(() =>
            {
                SetDescriptionAndRequirementData(tower);
                if (lastedCurrentTowerSoOnPlatform) 
                    UpdatePage(lastedMaxTierBuilded,lastedCurrentTowerSoOnPlatform,lastedCurrentTowerHpPercentage);
            });
            _towerUIItems.Add(towerUIItem);
        }
        
        for(int i = 0; i < requirementContent.childCount; i++) Destroy(requirementContent.GetChild(i).gameObject);
        for (int i = 0; i < 10; i++)
        {
            RequireUIItem item = Instantiate(requirementItemPrefab, requirementContent);
            item.gameObject.SetActive(false);
            _requireUIItems.Add(item);
        }
        currentTowerItemOnPage = towerRecipes[0];
        SetDescriptionAndRequirementData(currentTowerItemOnPage);
    }

    private void SetDescriptionAndRequirementData(TowerSO towerSo, List<InventoryItem> requirementItems = null)
    {
        towerImage.sprite = towerSo.towerImage;
        towerName.text = towerSo.towerName;
        towerDescription.text = towerSo.towerDescription;
        towerCost.text = GameManager.Instance.totalPoint >= towerSo.cost? $"<color=#05B900> Cost: ${towerSo.cost}": $"<color=red> Cost: ${towerSo.cost}";
        totalCoin.text = $"{GameManager.Instance.totalPoint}";

        if (requirementItems == null)
            requirementItems = towerSo.requireItems.ToList();

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
            currentTowerItemOnPage = towerSo;
        }
    }

    // ReSharper disable Unity.PerformanceAnalysis
    public void UpdatePage(int maxTierBuilded, TowerSO currentTowerSo, float currentHpPercentage)
    {
        buildButton.gameObject.SetActive(true);
        if (!currentTowerSo)
        {
            buildButtonText.text = "Build";
            buildTowerState = BuildTowerState.Build;
            foreach (TowerUIItem tower in _towerUIItems)
            {
                if (tower.TowerRecipe.tier > maxTierBuilded+1) tower.CloseButton();
                else tower.OpenButton();
                tower.SetCorrectIcon(tower.TowerRecipe.CheckRecipe() && GameManager.Instance.totalPoint >= tower.TowerRecipe.cost && tower.TowerRecipe.tier <= maxTierBuilded+1);
            }
            if(currentTowerItemOnPage) 
                SetDescriptionAndRequirementData(currentTowerItemOnPage);
        }
        else if (currentTowerItemOnPage.tier > currentTowerSo.tier)
        {
            buildButtonText.text = "Upgrade";
            buildTowerState = BuildTowerState.Upgrade;
            foreach (TowerUIItem tower in _towerUIItems)
            {
                if (tower.TowerRecipe.tier > maxTierBuilded+1) tower.CloseButton();
                else tower.OpenButton();
                tower.SetCorrectIcon(tower.TowerRecipe.CheckRecipe() && GameManager.Instance.totalPoint >= tower.TowerRecipe.cost && tower.TowerRecipe.tier <= maxTierBuilded+1);
            }
            if(currentTowerItemOnPage) 
                SetDescriptionAndRequirementData(currentTowerItemOnPage);
        }
        else if (currentTowerSo == currentTowerItemOnPage)
        {
            buildButtonText.text = "Repair";
            buildTowerState = BuildTowerState.Repair;
            foreach (TowerUIItem tower in _towerUIItems)
            {
                if (tower.TowerRecipe.tier > maxTierBuilded+1) tower.CloseButton();
                else tower.OpenButton();
                tower.SetCorrectIcon(tower.TowerRecipe.CheckRepairRecipe(currentHpPercentage) && GameManager.Instance.totalPoint >= tower.TowerRecipe.GetRepairState(currentHpPercentage).repairCost && tower.TowerRecipe.tier <= maxTierBuilded+1);
            }
            if(currentTowerItemOnPage) 
                SetDescriptionAndRequirementData(currentTowerItemOnPage, currentTowerItemOnPage.GetRepairState(currentHpPercentage).repairItems.ToList());
        }
        else
            buildButton.gameObject.SetActive(false);
        
        lastedMaxTierBuilded = maxTierBuilded;
        lastedCurrentTowerSoOnPlatform = currentTowerSo;
        lastedCurrentTowerHpPercentage = currentHpPercentage;
    }
    
    private void BuildTower()
    {
        OnBuildTower?.Invoke();
    }
}
