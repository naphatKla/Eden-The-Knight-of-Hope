using System;
using System.Collections.Generic;
using System.Linq;
using Inventory;
using UnityEngine;
using UnityEngine.UI;

[Serializable] public struct EquipementBGSlot
{
    public Sprite sprite;
    public ItemSlotType itemSlotType;
}
public class UIEquipmentInventoryPage : BaseUIInventoryPage
{
    [SerializeField] private UIInventoryDescription itemDescription; 
    [SerializeField] private List<ItemSlotType> listOfItemTypes = new List<ItemSlotType>();
    [HideInInspector] public List<UIEquipmentInventoryItem> lisOfQuickSlots = new List<UIEquipmentInventoryItem>();
    [SerializeField] private List<EquipementBGSlot> listOfBGSlots = new List<EquipementBGSlot>();

    public override void InitializeInventoryUI(InventorySo inventoryData)
    {
        base.InitializeInventoryUI(inventoryData);
        for (int i = 0; i < inventoryData.Size; i++)
        {
            listOfUIItems[i].itemSlotType = listOfItemTypes[i];
            listOfUIItems[i].GetComponent<UIEquipmentInventoryItem>().BGSlot.sprite = listOfBGSlots.Find(slot => slot.itemSlotType == listOfItemTypes[i]).sprite;
            if (listOfItemTypes[i] != ItemSlotType.QuickSlot) continue;
            lisOfQuickSlots.Add(listOfUIItems[i] as UIEquipmentInventoryItem);
        }
    }
    
    /// <summary>
    /// Reset the selection in the inventory UI page.
    /// </summary>
    public override void ResetSelection()
    {
        itemDescription.ResetDescription();
        DeselectAllItem();
    }
        
    /// <summary>
    /// Update the description in the inventory UI page.
    /// </summary>
    /// <param name="itemIndex">Item index target.</param>
    /// <param name="itemImage">Item sprite.</param>
    /// <param name="name">Item name.</param>
    /// <param name="description">Item description.</param>
    public void UpdateDescription(int itemIndex, Sprite itemImage, string name, string description)
    {
        itemDescription.SetDescription(itemImage, name, description);
        DeselectAllItem();
        otherPages.ForEach(page => page.DeselectAllItem());
        listOfUIItems[itemIndex].Select();
    }
}
