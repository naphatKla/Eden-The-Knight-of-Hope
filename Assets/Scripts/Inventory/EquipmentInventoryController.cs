using Inventory;

public class EquipmentInventoryController : BaseInventoryController<UIEquipmentInventoryPage>
{
    protected override void PrepareUIEvent()
    {
        base.PrepareUIEvent();
        inventoryUI.OnDescriptionRequested += HandleDescriptionRequest;
    }

    /// <summary>
    /// send the description of the item to the inventory UI page.
    /// </summary>
    /// <param name="itemIndex">Item index in the inventory</param>
    private void HandleDescriptionRequest(int itemIndex)
    {
        InventoryItem inventoryItem = inventoryData.GetItemAt(itemIndex);
        if (inventoryItem.IsEmpty)
        {
            inventoryUI.ResetSelection();
            return;
        }

        ItemSo item = inventoryItem.item;
        inventoryUI.UpdateDescription(itemIndex, item.ItemImage, item.name, item.Description);
    }
}
