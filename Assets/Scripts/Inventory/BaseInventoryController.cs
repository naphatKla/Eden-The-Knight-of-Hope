using System.Collections.Generic;
using UnityEngine;
namespace Inventory
{
    public class BaseInventoryController<T> : MonoBehaviour where T : BaseUIInventoryPage
    {
        [SerializeField] protected T inventoryUI;
        [SerializeField] protected InventorySo inventoryData;
        public List<InventoryItem> initialItems = new List<InventoryItem>();

        public virtual void Start()
        {
            PrepareUIEvent();
            PrepareInventoryData();
        }
        
        #region Methods

        /// <summary>
        /// Initialize the inventory UI page and subscribe to its events.
        /// </summary>
        protected virtual void PrepareUIEvent()
        {
            inventoryUI.InitializeInventoryUI(inventoryData);
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
        protected virtual void UpdateInventoryUI(Dictionary<int, InventoryItem> inventoryState)
        {
            inventoryUI.ResetAllItems();
            foreach (var item in inventoryState)
            {
                inventoryUI.UpdateData(item.Key, item.Value.item, item.Value.quantity);
            }
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