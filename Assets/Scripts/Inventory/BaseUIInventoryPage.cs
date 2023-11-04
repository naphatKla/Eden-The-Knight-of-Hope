using System;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory
{
    public class BaseUIInventoryPage : MonoBehaviour
    {
        [SerializeField] protected UIInventoryItem itemPrefab;
        [Header("Panel")] [SerializeField] protected RectTransform contentPanel;
        [SerializeField] protected MouseFollower mouseFollower;
        protected List<UIInventoryItem> _listOfUIItems = new List<UIInventoryItem>();
        protected int currentlyDraggedItemIndex = -1; // -1 means no item is being dragged
        public event Action<int> OnDescriptionRequested, OnItemActionRequested, OnStarDragging;
        public event Action<int, int> OnSwapItem;
        public virtual void Awake()
        {
            mouseFollower.Toggle(false);
        }

        #region Methods

        /// <summary>
        /// Initialize items in the inventory UI page and subscribe to its events.
        /// </summary>
        /// <param name="inventoryData">Inventory SO of the inventory.</param>
        public virtual void InitializeInventoryUI(InventorySo inventoryData)
        {
            for (int i = 0; i < inventoryData.Size; i++)
            {
                UIInventoryItem uiItem = Instantiate(itemPrefab, Vector3.zero, Quaternion.identity);
                uiItem.transform.SetParent(contentPanel);
                uiItem.ParentInventoryData = inventoryData;
                uiItem.Index = i;
                _listOfUIItems.Add(uiItem);

                uiItem.OnItemClicked += HandleItemSelection;
                uiItem.OnItemBeginDrag += HandleBeginDrag;
                uiItem.OnItemDroppedOn += HandleSwap;
                uiItem.OnItemEndDrag += HandleEndDrag;
                uiItem.OnRightMouseBtnClick += HandleShowItemActions;
            }
        }

        /// <summary>
        /// Update the item data in the inventory UI page.
        /// </summary>
        /// <param name="itemIndex">Item index.</param>
        /// <param name="itemImage">Item image.</param>
        /// <param name="itemQuantity">Item amount.</param>
        /// <param name="itemSlotType">Item type.</param>
        public void UpdateData(int itemIndex, Sprite itemImage, int itemQuantity, ItemSlotType itemSlotType)
        {
            if (_listOfUIItems.Count <= itemIndex) return;
            _listOfUIItems[itemIndex].SetData(itemImage, itemQuantity, itemSlotType);
        }

        /// <summary>
        /// Create the dragged item.
        /// </summary>
        /// <param name="sprite">Dragged item sprite.</param>
        /// <param name="quantity">Dragged item amount.</param>
        public void CreateDraggedItem(Sprite sprite, int quantity)
        {
            mouseFollower.Toggle(true);
            mouseFollower.SetData(sprite, quantity);
        }

        /// <summary>
        /// Reset the selection in the inventory UI page.
        /// </summary>
        public virtual void ResetSelection()
        {
            DeselectAllItem();
        }

        /// <summary>
        /// Show the inventory UI page.
        /// </summary>
        public void Show()
        {
            gameObject.SetActive(true);
            ResetSelection();
        }

        /// <summary>
        /// Hide the inventory UI page.
        /// </summary>
        public void Hide()
        {
            gameObject.SetActive(false);
        }
        
        /// <summary>
        /// Reset all items in the inventory UI page.
        /// </summary>
        public void ResetAllItems()
        {
            foreach (var item in _listOfUIItems)
            {
                item.ResetData();
                item.Deselect();
            }
        }

        /// <summary>
        /// Handle the item selection.
        /// </summary>
        /// <param name="inventoryItemUI">Item target</param>
        private void HandleItemSelection(UIInventoryItem inventoryItemUI)
        {
            int index = _listOfUIItems.IndexOf(inventoryItemUI);
            if (index == -1) return;
            OnDescriptionRequested?.Invoke(index);
        }

        /// <summary>
        /// Handle the item begin drag.
        /// </summary>
        /// <param name="inventoryItemUI">Item target</param>
        private void HandleBeginDrag(UIInventoryItem inventoryItemUI)
        {
            int index = _listOfUIItems.IndexOf(inventoryItemUI);
            if (index == -1) return;
            currentlyDraggedItemIndex = index;
            HandleItemSelection(inventoryItemUI);
            OnStarDragging?.Invoke(index);
        }

        /// <summary>
        /// Handle the item swap.
        /// </summary>
        /// <param name="inventoryItemUI">Item target</param>
        private void HandleSwap(UIInventoryItem inventoryItemUI)
        {
            int index = _listOfUIItems.IndexOf(inventoryItemUI);
            if (index == -1 || currentlyDraggedItemIndex == -1) return;
            OnSwapItem?.Invoke(currentlyDraggedItemIndex, index);
        }
        
        /// <summary>
        /// Handle the item end drag.
        /// </summary>
        /// <param name="inventoryItemUI">Item target</param>
        private void HandleEndDrag(UIInventoryItem inventoryItemUI)
        {
            ResetDraggedItem();
        }

        private void HandleShowItemActions(UIInventoryItem inventoryItemUI)
        {
        }

        /// <summary>
        /// Reset the dragged item.
        /// </summary>
        private void ResetDraggedItem()
        {
            mouseFollower.Toggle(false);
            currentlyDraggedItemIndex = -1;
        }

        /// <summary>
        /// Deselect all items in the inventory UI page.
        /// </summary>
        protected void DeselectAllItem()
        {
            foreach (UIInventoryItem item in _listOfUIItems) item.Deselect();
        }

        #endregion
    }
}