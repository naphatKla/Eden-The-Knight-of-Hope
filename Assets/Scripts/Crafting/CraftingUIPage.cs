using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
public enum CraftingType
{
    Helmets,
    Armors,
    Leggings,
    Boots,
    Weapons,
    Materials,
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

    public void Initialize(List<CraftingItem> craftingItems)
    {
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
               _craftingUIItems.Add(craftingUIItem);
           }
        }
    }

    public void UpdatePage()
    {
        _craftingUIItems.ForEach(item => item.SetCorrectIcon(item.CraftingRecipe.CheckRecipe()));
    }
}
