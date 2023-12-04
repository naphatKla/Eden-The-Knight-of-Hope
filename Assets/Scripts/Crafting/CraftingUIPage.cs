using System;
using System.Collections.Generic;
using System.Linq;
using Inventory;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum CraftingType
{
    Helmets,
    Armors,
    Leggings,
    Boots,
    Weapons,
    Materials,
    Potions,
}

[Serializable] public struct CraftingItem
{
    public CraftingRecipeSO craftingRecipe;
    public CraftingType craftingType;
}
public class CraftingUIPage : MonoBehaviour
{
    [Header("ItemZone")]
    [SerializeField] private Transform itemContent;
    [SerializeField] private CraftingUIItem craftingItemPrefab;
    [SerializeField] private TextMeshProUGUI craftingHeaderPrefab;
    private CraftingType[] _craftingTypesHeader;
    private List<CraftingUIItem> _craftingUIItems = new List<CraftingUIItem>();
    
    [Header("DetailsZone")]
    [SerializeField] private Image itemIcon;
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI itemDescription;
    
    [Header("RequirementZone")]
    [SerializeField] private Transform requirementContent;
    [SerializeField] private RequireUIItem requirementItemPrefab;
    private List<RequireUIItem> _requireUIItems = new List<RequireUIItem>();
    private CraftingItem _currentCraftingItem;
    private Color _craftButtonDefaultColor;
    
    [SerializeField] private Button craftButton;

    private void Awake()
    {
        _craftButtonDefaultColor = new Color(0.386788f, 1, 0.2877358f, 1);
    }

    public void Initialize(List<CraftingItem> craftingItems)
    {
        craftButton.onClick.AddListener(Craft);
        _craftingTypesHeader = craftingItems.Select(item => item.craftingType).Distinct().ToArray();
        for(int i = 0; i < itemContent.childCount; i++) Destroy(itemContent.GetChild(i).gameObject);
        
        foreach (CraftingType type in _craftingTypesHeader)
        {
           Instantiate(craftingHeaderPrefab, itemContent).text = type.ToString();

           foreach (CraftingItem item in craftingItems)
           {
               if (item.craftingType != type) continue;
               CraftingUIItem craftingUIItem = Instantiate(craftingItemPrefab, itemContent);
               craftingUIItem.SetData(item.craftingRecipe);
                craftingUIItem.GetComponent<Button>().onClick.AddListener(() => SetDescriptionAndRequirementData(item));
               _craftingUIItems.Add(craftingUIItem);
           }
        }
        
        for(int i = 0; i < requirementContent.childCount; i++) Destroy(requirementContent.GetChild(i).gameObject);
        for (int i = 0; i < 10; i++)
        {
            RequireUIItem item = Instantiate(requirementItemPrefab, requirementContent);
            item.gameObject.SetActive(false);
            _requireUIItems.Add(item);
        }
        _currentCraftingItem = craftingItems[0];
    }

    private void SetDescriptionAndRequirementData(CraftingItem craftingItem)
    {
        itemIcon.sprite = craftingItem.craftingRecipe.Result.item.ItemImage;
        itemName.text = craftingItem.craftingRecipe.Result.item.name;
        itemDescription.text = craftingItem.craftingRecipe.Result.item.Description;

        List<InventoryItem> requirementItems = craftingItem.craftingRecipe.RequireItems.ToList();

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
            _currentCraftingItem = craftingItem;
            if (_currentCraftingItem.craftingRecipe.CheckRecipe())
            {
                craftButton.enabled = true;
                craftButton.GetComponent<Image>().color = _craftButtonDefaultColor;
                craftButton.GetComponentInChildren<TextMeshProUGUI>().color = new Color(0.1960784f, 0.1960784f, 0.1960784f, 1);
            }
            else
            {
                craftButton.enabled = false;
                craftButton.GetComponent<Image>().color = new Color(0,0,0,0.5f);
                craftButton.GetComponentInChildren<TextMeshProUGUI>().color = new Color(0.1960784f, 0.1960784f, 0.1960784f, 0.35f);
            }
        }
    }

    public void UpdatePage()
    {
        _craftingUIItems.ForEach(item => item.SetCorrectIcon(item.CraftingRecipe.CheckRecipe()));
        if (_currentCraftingItem.craftingRecipe)
        {
            SetDescriptionAndRequirementData(_currentCraftingItem);
        }
    }

    private void Craft()
    {
        InventorySo inventoryData = PlayerInventoryController.Instance.InventoryData;
        if (!_currentCraftingItem.craftingRecipe.CheckRecipe()) return;
        inventoryData.AddItem(_currentCraftingItem.craftingRecipe.Result.item, _currentCraftingItem.craftingRecipe.Result.quantity);
        foreach (InventoryItem requireItem in _currentCraftingItem.craftingRecipe.RequireItems)
        {
            inventoryData.RemoveItem(requireItem.item, requireItem.quantity);
        }
        UpdatePage();
    }
}
