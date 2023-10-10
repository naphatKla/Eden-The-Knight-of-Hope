using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

[CreateAssetMenu]
public class InventorySO : ScriptableObject
{
    [SerializeField] 
    private List<InventoryItem> inventoryItem;
    
    [field: SerializeField]
    public int Size { get;private set; } = 10;
    
    public void Initialize()
    {
        inventoryItem = new List<InventoryItem>();
        for (int i = 0; i < Size; i++)
        {
            inventoryItem.Add(InventoryItem.GetEmptyItem());
        }
    }
    public void  AddItem(ItemSO item, int quantity)
    {
        for (int i = 0; i < inventoryItem.Count; i++)
        {
            if (inventoryItem[i].IsEmpty)
            {
                inventoryItem[i] = new InventoryItem
                {
                    item = item,
                    quantity = quantity
                };
            }
        }
    }

    public Dictionary<int, InventoryItem> GetCurrentInventory()
    {
        Dictionary<int, InventoryItem> returnValue = 
            new Dictionary<int, InventoryItem>();
        for (int i = 0; i < inventoryItem.Count; i++)
        {
            if (inventoryItem[i].IsEmpty)
                continue;
            returnValue[i] = inventoryItem[i];
        }

        return returnValue;
    }

    public InventoryItem GetItemAT(int itemIndex)
    {
        return inventoryItem[itemIndex];
    }
}

[Serializable]
public struct InventoryItem
{
    public int quantity;
    public ItemSO item;
    
    public bool IsEmpty => item == null;
    
    public InventoryItem ChangeQuantity(int newQuantity)
    {
        return new InventoryItem
        {
            item = this.item,
            quantity = newQuantity
        };
    }
    
    public static InventoryItem GetEmptyItem()
    => new InventoryItem
    {
        item = null,
        quantity = 0,
    };
}
