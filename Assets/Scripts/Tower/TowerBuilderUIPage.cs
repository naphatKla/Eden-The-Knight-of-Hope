using System;
using System.Collections.Generic;
using System.Linq;
using Interaction;
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
    private Color _buildButtonDefaultColor;
    public TowerPlatform TowerPlatformLinked { get; set; }

    private void Awake()
    {
        closeButton.onClick.AddListener(() => gameObject.transform.parent.gameObject.SetActive(false));
        _buildButtonDefaultColor = buildButton.GetComponent<Image>().color;
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

    private void SetDescriptionAndRequirementData(TowerSO towerSo, List<InventoryItem> requirementItems = null, int cost = -1)
    {
        if (cost == -1)
            cost = towerSo.cost;
        towerImage.sprite = towerSo.towerImage;
        towerName.text = towerSo.towerName;
        towerDescription.text = towerSo.towerDescription;
        towerCost.text = GameManager.Instance.totalPoint >= cost? $"<color=#05B900> Cost: ${cost}": $"<color=red> Cost: ${cost}";
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
        buildButton.GetComponent<Button>().enabled = true;
        buildButton.GetComponent<Image>().color = _buildButtonDefaultColor;
        buildButton.GetComponentInChildren<TextMeshProUGUI>().color = new Color(0.1960784f, 0.1960784f, 0.1960784f, 1);
        if (!currentTowerSo)
        {
            buildButtonText.text = "Build";
            buildTowerState = BuildTowerState.Build;
            foreach (TowerUIItem tower in _towerUIItems)
            {
                if (tower.TowerRecipe.tier > maxTierBuilded) tower.CloseButton();
                else tower.OpenButton();
                tower.SetCorrectIcon(tower.TowerRecipe.CheckRecipe() && GameManager.Instance.totalPoint >= tower.TowerRecipe.cost && tower.TowerRecipe.tier <= maxTierBuilded);
                if (tower.TowerRecipe.tier == 1)
                {
                    tower.SetCorrectIcon(tower.TowerRecipe.CheckRecipe() && GameManager.Instance.totalPoint >= tower.TowerRecipe.cost);
                    tower.OpenButton();
                }
            }
            if(currentTowerItemOnPage) 
                SetDescriptionAndRequirementData(currentTowerItemOnPage);
            
            int cost = currentTowerItemOnPage.cost;
            if (!currentTowerItemOnPage.CheckRecipe() ||
                GameManager.Instance.totalPoint < cost)
            {
                buildButton.GetComponent<Button>().enabled = false;
                buildButton.GetComponent<Image>().color = new Color(0,0,0,0.5f);
                buildButton.GetComponentInChildren<TextMeshProUGUI>().color = new Color(0.1960784f, 0.1960784f, 0.1960784f, 0.35f);
            }
        }
        else if (currentTowerItemOnPage.tier > currentTowerSo.tier)
        {
            buildButtonText.text = "Upgrade";
            buildTowerState = BuildTowerState.Upgrade;
            foreach (TowerUIItem tower in _towerUIItems)
            {
                if (tower.TowerRecipe.tier > maxTierBuilded+1 || tower.TowerRecipe.tier < currentTowerSo.tier) tower.CloseButton();
                else tower.OpenButton();
                if (tower.TowerRecipe.tier == currentTowerSo.tier+1)
                    tower.SetCorrectIcon(tower.TowerRecipe.CheckRecipe() && GameManager.Instance.totalPoint >= tower.TowerRecipe.cost && tower.TowerRecipe.tier <= maxTierBuilded+1);
                if (tower.TowerRecipe == currentTowerSo)
                    tower.SetCurrentIcon(true);
            }
            if(currentTowerItemOnPage) 
                SetDescriptionAndRequirementData(currentTowerItemOnPage);
            
            int cost = currentTowerItemOnPage.cost;
            if (!currentTowerItemOnPage.CheckRecipe() ||
                GameManager.Instance.totalPoint < cost)
            {
                buildButton.GetComponent<Button>().enabled = false;
                buildButton.GetComponent<Image>().color = new Color(0,0,0,0.5f);
                buildButton.GetComponentInChildren<TextMeshProUGUI>().color = new Color(0.1960784f, 0.1960784f, 0.1960784f, 0.35f);
            }
        }
        else if (currentTowerSo == currentTowerItemOnPage)
        {
            buildButtonText.text = "Repair";
            buildTowerState = BuildTowerState.Repair;
            foreach (TowerUIItem tower in _towerUIItems)
            {
                if (tower.TowerRecipe.tier > maxTierBuilded+1 || tower.TowerRecipe.tier < currentTowerSo.tier) tower.CloseButton();
                else tower.OpenButton();
                if (tower.TowerRecipe.tier == currentTowerSo.tier+1)
                    tower.SetCorrectIcon(tower.TowerRecipe.CheckRecipe() && GameManager.Instance.totalPoint >= tower.TowerRecipe.cost && tower.TowerRecipe.tier <= maxTierBuilded+1);
                if (tower.TowerRecipe == currentTowerSo)
                    tower.SetCurrentIcon(true);
            }
            if(currentTowerItemOnPage) 
                SetDescriptionAndRequirementData(currentTowerItemOnPage, currentTowerItemOnPage.GetRepairState(currentHpPercentage).repairItems.ToList(), currentTowerItemOnPage.GetRepairState(currentHpPercentage).repairCost);
            
            TowerHealthSystem _towerHealthSystem = TowerPlatformLinked.towerOnPlatform.GetComponent<TowerHealthSystem>();
            float towerHpPercentage = _towerHealthSystem ? (_towerHealthSystem.CurrentHp / _towerHealthSystem.maxHp) * 100 : 0;
            int cost = currentTowerItemOnPage.GetRepairState(towerHpPercentage).repairCost;
            if (!currentTowerItemOnPage.CheckRepairRecipe(towerHpPercentage) ||
                GameManager.Instance.totalPoint < cost || towerHpPercentage >= 100)
            {
                buildButton.GetComponent<Button>().enabled = false;
                buildButton.GetComponent<Image>().color = new Color(0,0,0,0.5f);
                buildButton.GetComponentInChildren<TextMeshProUGUI>().color = new Color(0.1960784f, 0.1960784f, 0.1960784f, 0.35f);
            }
        }
        else
        {
            buildButton.gameObject.SetActive(false);
        }
        
        lastedMaxTierBuilded = maxTierBuilded;
        lastedCurrentTowerSoOnPlatform = currentTowerSo;
        lastedCurrentTowerHpPercentage = currentHpPercentage;
    }
    
    private void BuildTower()
    {
        OnBuildTower?.Invoke();
    }
}
