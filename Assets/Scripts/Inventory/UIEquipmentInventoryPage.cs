using System.Collections.Generic;
using Inventory;
using UnityEngine;

public class UIEquipmentInventoryPage : BaseUIInventoryPage
{
    [SerializeField] private UIInventoryDescription itemDescription; 
    [SerializeField] private List<ItemSlotType> listOfItemTypes = new List<ItemSlotType>();
    public override void InitializeInventoryUI(InventorySo inventoryData)
    {
        base.InitializeInventoryUI(inventoryData);
        for (int i = 0; i < inventoryData.Size; i++)
        {
            listOfUIItems[i].itemSlotType = listOfItemTypes[i];
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
