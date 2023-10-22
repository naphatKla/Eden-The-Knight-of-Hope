using System.Collections.Generic;
using UnityEngine;

namespace Inventory
{
    public class InventoryController : MonoBehaviour
    {
        [SerializeField] private UIInventoryPage inventoryUI;
        [SerializeField] private InventorySo inventoryData;
        [SerializeField] private KeyCode key;
        public List<InventoryItem> initialItems = new List<InventoryItem>();

        public void Start()
        {
            PrepareUIEvent();
            PrepareInventoryData();
        }

        public void Update()
        {
            // press 0 to add item for testing
            if (Input.GetKeyDown(KeyCode.Alpha0))
            {
                inventoryData.AddItem(initialItems[0].item, 20);
                Debug.Log("Add");
            }

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

        #region Methods

        /// <summary>
        /// Initialize the inventory UI page and subscribe to its events.
        /// </summary>
        private void PrepareUIEvent()
        {
            inventoryUI.InitializeInventoryUI(inventoryData.Size);
            inventoryUI.OnDescriptionRequested += HandleDescriptionRequest;
            inventoryUI.OnSwapItem += HandleSwapItem;
            inventoryUI.OnStarDragging += HandleDragging;
            inventoryUI.OnItemActionRequested += HandleItemActionRequest;
        }

        /// <summary>
        /// Initialize the inventory data and subscribe to its events.
        /// </summary>
        private void PrepareInventoryData()
        {
            inventoryData.Initialize();
            inventoryData.OnInventoryUpdated += UpdateInventoryUI;
            foreach (InventoryItem item in initialItems)
            {
                if (item.IsEmpty) continue;
                inventoryData.AddItem(item);
            }
        }

        /// <summary>
        /// Update the inventory UI page.
        /// </summary>
        /// <param name="inventoryState"></param>
        private void UpdateInventoryUI(Dictionary<int, InventoryItem> inventoryState)
        {
            inventoryUI.ResetAllItems();
            foreach (var item in inventoryState)
            {
                inventoryUI.UpdateData(item.Key, item.Value.item.ItemImage, item.Value.quantity);
            }
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

        /// <summary>
        /// Swap 2 items in the inventory.
        /// </summary>
        /// <param name="itemIndex1">Item one</param>
        /// <param name="itemIndex2">Item two</param>
        private void HandleSwapItem(int itemIndex1, int itemIndex2)
        {
            inventoryData.SwapItems(itemIndex1, itemIndex2);
        }

        /// <summary>
        /// Handle the dragging of the item.
        /// </summary>
        /// <param name="itemIndex">Index of dragging item.</param>
        private void HandleDragging(int itemIndex)
        {
            InventoryItem inventoryItem = inventoryData.GetItemAt(itemIndex);
            if (inventoryItem.IsEmpty) return;
            inventoryUI.CreateDraggedItem(inventoryItem.item.ItemImage, inventoryItem.quantity);
        }

        private void HandleItemActionRequest(int itemIndex)
        {
        }

        #endregion
    }
}