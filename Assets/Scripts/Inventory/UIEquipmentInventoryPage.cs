using System.Collections.Generic;
using Inventory;
using UnityEngine;
using UnityEngine.Serialization;

public class UIEquipmentInventoryPage : BaseUIInventoryPage
{
    [SerializeField] private List<ItemSlotType> listOfItemTypes = new List<ItemSlotType>();
    public override void InitializeInventoryUI(InventorySo inventoryData)
    {
        base.InitializeInventoryUI(inventoryData);
        for (int i = 0; i < inventoryData.Size; i++)
        {
            _listOfUIItems[i].itemSlotType = listOfItemTypes[i];
        }
    }
}
