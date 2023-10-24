using UnityEngine;

namespace Inventory
{
    public class PlayerInventoryController : BaseInventoryController<PlayerUIInventoryPage>
    {
        [SerializeField] private KeyCode key;
        public void Update()
        {
            // press 0 to add item for testing
            if (Input.GetKeyDown(KeyCode.Alpha0))
                inventoryData.AddItem(initialItems[0].item, 20);
            
            if (!Input.GetKeyDown(key)) return;
            if (inventoryUI.isActiveAndEnabled)
            {
                inventoryUI.Hide();
                return;
            }

            inventoryUI.Show();
            foreach (var item in inventoryData.GetCurrentInventoryState())
                inventoryUI.UpdateData(item.Key, item.Value.item.ItemImage, item.Value.quantity);
        }

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
}