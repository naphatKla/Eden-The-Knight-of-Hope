using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

namespace Inventory
{
    [CreateAssetMenu]
    public class InventorySo : ScriptableObject
    {
        [SerializeField] private List<InventoryItem> inventoryItems;
        [field: SerializeField] public int Size { get; private set; } = 10;
        public event Action<Dictionary<int, InventoryItem>> OnInventoryUpdated;

        #region Methods

        /// <summary>
        /// Initialize the inventory data.
        /// </summary>
        public void Initialize()
        {
            inventoryItems = new List<InventoryItem>();
            for (int i = 0; i < Size; i++) inventoryItems.Add(InventoryItem.GetEmptyItem());
        }

        /// <summary>
        /// Add an item to the inventory.
        /// </summary>
        /// <param name="item">Item to add.</param>
        /// <param name="quantity">Amount of added item.</param>
        public void AddItem(ItemSo item, int quantity)
        {
            if (quantity == 0) return;
            if (!item) return;
            int quantityLeftToAdd = quantity;

            for (int i = 0; i < inventoryItems.Count; i++)
            {
                if (item == inventoryItems[i].item)
                {
                    int addedQuantity = inventoryItems[i].quantity + quantity;
                    if (addedQuantity <= item.MaxStackSize)
                    {
                        inventoryItems[i] = inventoryItems[i].ChangeQuantity(addedQuantity);
                        InventoryFeedback.Instance.ShowFeedback(item.ItemImage, quantity);
                        InformAboutChange();
                        return;
                    }

                    quantityLeftToAdd = addedQuantity - item.MaxStackSize;
                    inventoryItems[i] = inventoryItems[i].ChangeQuantity(item.MaxStackSize);
                    InformAboutChange();
                }

                if (!inventoryItems[i].IsEmpty) continue;
                if (quantityLeftToAdd > item.MaxStackSize)
                {
                    inventoryItems[i] = new InventoryItem { item = item, quantity = item.MaxStackSize };
                    InformAboutChange();
                    quantityLeftToAdd -= item.MaxStackSize;
                    continue;
                }

                inventoryItems[i] = new InventoryItem { item = item, quantity = quantityLeftToAdd };
                InventoryFeedback.Instance.ShowFeedback(item.ItemImage, quantity);
                InformAboutChange();
                return;
            }
        }

        public void AddItem(InventoryItem item)
        {
            AddItem(item.item, item.quantity);
        }

        /// <summary>
        /// Get the current inventory state (What items and how many item in the inventory).
        /// </summary>
        /// <returns>Current inventory state and data.</returns>
        public Dictionary<int, InventoryItem> GetCurrentInventoryState()
        {
            Dictionary<int, InventoryItem> returnValue = new Dictionary<int, InventoryItem>();

            for (int i = 0; i < inventoryItems.Count; i++)
            {
                if (inventoryItems[i].IsEmpty) continue;
                returnValue[i] = inventoryItems[i];
            }

            return returnValue;
        }

        /// <summary>
        /// Get the item from the inventory at the given index.
        /// </summary>
        /// <param name="itemIndex">Index to get.</param>
        /// <returns>Item at the given index.</returns>
        public InventoryItem GetItemAt(int itemIndex)
        {
            return inventoryItems[itemIndex];
        }
        
        public List<InventoryItem> GetAllItems()
        {
            return inventoryItems;
        }
        
        public int GetAllQuantityOfItem(InventoryItem item)
        {
            return inventoryItems.FindAll(i => i.item == item.item).Sum(i=>i.quantity);
        }

        /// <summary>
        /// Swap 2 items in the inventory (Include moving one item to the empty slot).
        /// </summary>
        /// <param name="itemIndex1">Item one</param>
        /// <param name="itemIndex2">Item two</param>
        public void SwapItems(int itemIndex1, int itemIndex2)
        {
            (inventoryItems[itemIndex1], inventoryItems[itemIndex2]) =
                (inventoryItems[itemIndex2], inventoryItems[itemIndex1]);
            InformAboutChange();
        }
        
        public void SwapItemsMoveBetweenInventories(InventorySo otherInventory, int itemIndex1, int itemIndex2)
        {
            (inventoryItems[itemIndex1], otherInventory.inventoryItems[itemIndex2]) =
                (otherInventory.inventoryItems[itemIndex2], inventoryItems[itemIndex1]);
            InformAboutChange();
            otherInventory.InformAboutChange();
        }
        
        public void SortInventory()
        {
            inventoryItems.Sort((item1, item2) =>
            {
                if (item1.IsEmpty && item2.IsEmpty) return 0;
                if (item1.IsEmpty) return 1;
                if (item2.IsEmpty) return -1;
                return item1.item.name.CompareTo(item2.item.name);
            });
            InformAboutChange();
        }
        
        public void RemoveItem(int itemIndex, int quantity)
        {
            if (inventoryItems[itemIndex].IsEmpty) return;
            if (inventoryItems[itemIndex].quantity <= quantity)
            {
                InventoryFeedback.Instance.ShowFeedback(inventoryItems[itemIndex].item.ItemImage, -quantity);
                inventoryItems[itemIndex] = InventoryItem.GetEmptyItem();
                InformAboutChange();
                return;
            }

            inventoryItems[itemIndex] = inventoryItems[itemIndex].ChangeQuantity(inventoryItems[itemIndex].quantity - quantity);
            InventoryFeedback.Instance.ShowFeedback(inventoryItems[itemIndex].item.ItemImage, -quantity);
            InformAboutChange();
        }
        
        public void RemoveItem(ItemSo item, int quantity)
        {
            List<InventoryItem> itemsToRemove = inventoryItems.FindAll(inventoryItem => inventoryItem.item == item);
            int quantityLeftToRemove = quantity;
            for(int i = 0; i < itemsToRemove.Count; i++)
            {
                if (itemsToRemove[i].quantity <= quantityLeftToRemove)
                {
                    quantityLeftToRemove -= itemsToRemove[i].quantity;
                    inventoryItems[inventoryItems.IndexOf(itemsToRemove[i])] = InventoryItem.GetEmptyItem();
                    continue;
                }
                inventoryItems[inventoryItems.IndexOf(itemsToRemove[i])] = itemsToRemove[i].ChangeQuantity(itemsToRemove[i].quantity - quantityLeftToRemove);
                InventoryFeedback.Instance.ShowFeedback(item.ItemImage, -quantity);
                break;
            }
            InformAboutChange();
        }

        /// <summary>
        /// Update the inventory data.
        /// </summary>
        private void InformAboutChange()
        {
            OnInventoryUpdated?.Invoke(GetCurrentInventoryState());
        }

        #endregion
    }

    [Serializable]
    public struct InventoryItem
    {
        public int quantity;
        public ItemSo item;
        public bool IsEmpty => item == null;

        public InventoryItem ChangeQuantity(int newQuantity)
        {
            return new InventoryItem { item = this.item, quantity = newQuantity };
        }

        public static InventoryItem GetEmptyItem() => new InventoryItem { item = null, quantity = 0, };
    }
}