using System;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory
{
    public class UIInventoryPage : MonoBehaviour
    {
        [SerializeField] private UIInventoryItem itemPrefab;
        [SerializeField] private RectTransform contentPanel;
        [SerializeField] private UIInventoryDescription itemDescription;
        [SerializeField] private MouseFollower mouseFollower;
        private List<UIInventoryItem> _listOfUIItems = new List<UIInventoryItem>();
        private int _currentlyDraggedItemIndex = -1; // -1 means no item is being dragged
        public event Action<int> OnDescriptionRequested, OnItemActionRequested, OnStarDragging;
        public event Action<int, int> OnSwapItem;

        public void Awake()
        {
            mouseFollower.Toggle(false);
            itemDescription.ResetDescription();
        }

        #region Methods
        
        /// <summary>
        /// Initialize items in the inventory UI page and subscribe to its events.
        /// </summary>
        /// <param name="inventorySize">Size of the inventory.</param>
        public void InitializeInventoryUI(int inventorySize)
        {
            for (int i = 0; i < inventorySize; i++)
            {
                UIInventoryItem uiItem = Instantiate(itemPrefab, Vector3.zero, Quaternion.identity);
                uiItem.transform.SetParent(contentPanel);
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
        public void UpdateData(int itemIndex, Sprite itemImage, int itemQuantity)
        {
            if (_listOfUIItems.Count <= itemIndex) return;
            _listOfUIItems[itemIndex].SetData(itemImage, itemQuantity);
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
        public void ResetSelection()
        {
            itemDescription.ResetDescription();
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
            _listOfUIItems[itemIndex].Select();
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
            _currentlyDraggedItemIndex = index;
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
            if (index == -1 || _currentlyDraggedItemIndex == -1) return;
            OnSwapItem?.Invoke(_currentlyDraggedItemIndex, index);
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
            _currentlyDraggedItemIndex = -1;
        }
        
        
        /// <summary>
        /// Deselect all items in the inventory UI page.
        /// </summary>
        private void DeselectAllItem()
        {
            foreach (UIInventoryItem item in _listOfUIItems)
                item.Deselect();
        }
        #endregion
    }
}

