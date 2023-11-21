using System.Collections.Generic;
using System.Linq;
using Inventory;
using UnityEngine;

[CreateAssetMenu]
public class CraftingRecipeSO : ScriptableObject
{
    [SerializeField] private InventoryItem[] requireItems;
    [SerializeField] private InventoryItem result;

    public InventoryItem[] RequireItems => requireItems;
    public InventoryItem Result => result;
    
    public bool CheckRecipe()
    {
        List<InventoryItem> playerItems = PlayerInventoryController.Instance.InventoryData.GetAllItems();
        foreach (InventoryItem requireItem in requireItems)
        {
            if (playerItems.All(item => item.item != requireItem.item)) return false;
            if (playerItems.Where(item => item.item == requireItem.item).Sum(item => item.quantity) < requireItem.quantity) return false;
        }
        return true;
    }
}
