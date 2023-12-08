using System;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory
{
    public class BaseUIInventoryPage : MonoBehaviour
    {
        [SerializeField] protected BaseUIInventoryItem itemPrefab;
        [Header("Panel")] [SerializeField] protected RectTransform contentPanel;
        [SerializeField] protected MouseFollower mouseFollower;
        [SerializeField] protected List<BaseUIInventoryPage> otherPages; // inventory pages that open when this page is open
        [HideInInspector] public List<BaseUIInventoryItem> listOfUIItems = new List<BaseUIInventoryItem>();
        protected int currentlyDraggedItemIndex = -1; // -1 means no item is being dragged
        public event Action<int> OnDescriptionRequested, OnItemActionRequested, OnStarDragging;
        public event Action<int, int> OnSwapItem;
        
        [Header("Sound")] 
        [SerializeField] protected AudioClip[] openSound;
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
                BaseUIInventoryItem baseUIItem = Instantiate(itemPrefab, Vector3.zero, Quaternion.identity);
                baseUIItem.transform.SetParent(contentPanel);
                baseUIItem.ParentInventoryData = inventoryData;
                baseUIItem.Index = i;
                listOfUIItems.Add(baseUIItem);

                baseUIItem.OnItemClicked += HandleItemSelection;
                baseUIItem.OnItemBeginDrag += HandleBeginDrag;
                baseUIItem.OnItemDroppedOn += HandleSwap;
                baseUIItem.OnItemEndDrag += HandleEndDrag;
                baseUIItem.OnRightMouseBtnClick += HandleShowItemActions;
            }
        }

        /// <summary>
        /// Update the item data in the inventory UI page.
        /// </summary>
        /// <param name="itemIndex">Item index.</param>
        /// <param name="itemData">Item SO</param>
        /// <param name="itemQuantity">Item amount.</param>
        public void UpdateData(int itemIndex, ItemSo itemData, int itemQuantity)
        {
            if (listOfUIItems.Count <= itemIndex) return;
            listOfUIItems[itemIndex].SetData(itemData, itemQuantity);
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
        public void Show(bool waitForAnyUIClose = true)
        {
            if (UIManager.Instance.CheckIsAnyUIOpen() && waitForAnyUIClose) return;

            SoundManager.Instance.RandomPlaySound(openSound);
            gameObject.SetActive(true);
            ResetSelection();
        }

        /// <summary>
        /// Hide the inventory UI page.
        /// </summary>
        public virtual void Hide()
        {
            SoundManager.Instance.RandomPlaySound(openSound);
            gameObject.SetActive(false);
        }
        
        /// <summary>
        /// Reset all items in the inventory UI page.
        /// </summary>
        public virtual void ResetAllItems()
        {
            foreach (var item in listOfUIItems)
            {
                item.ResetData();
                item.Deselect();
            }
        }

        /// <summary>
        /// Handle the item selection.
        /// </summary>
        /// <param name="inventoryItemBaseUI">Item target</param>
        protected virtual void HandleItemSelection(BaseUIInventoryItem inventoryItemBaseUI)
        {
            int index = listOfUIItems.IndexOf(inventoryItemBaseUI);
            if (index == -1) return;
            OnDescriptionRequested?.Invoke(index);
        }

        /// <summary>
        /// Handle the item begin drag.
        /// </summary>
        /// <param name="inventoryItemBaseUI">Item target</param>
        private void HandleBeginDrag(BaseUIInventoryItem inventoryItemBaseUI)
        {
            int index = listOfUIItems.IndexOf(inventoryItemBaseUI);
            if (index == -1) return;
            currentlyDraggedItemIndex = index;
            HandleItemSelection(inventoryItemBaseUI);
            OnStarDragging?.Invoke(index);
        }

        /// <summary>
        /// Handle the item swap.
        /// </summary>
        /// <param name="inventoryItemBaseUI">Item target</param>
        protected virtual void HandleSwap(BaseUIInventoryItem inventoryItemBaseUI)
        {
            int index = listOfUIItems.IndexOf(inventoryItemBaseUI);
            if (index == -1 || currentlyDraggedItemIndex == -1) return;
            OnSwapItem?.Invoke(currentlyDraggedItemIndex, index);
        }
        
        /// <summary>
        /// Handle the item end drag.
        /// </summary>
        /// <param name="inventoryItemBaseUI">Item target</param>
        private void HandleEndDrag(BaseUIInventoryItem inventoryItemBaseUI)
        {
            ResetDraggedItem();
        }

        private void HandleShowItemActions(BaseUIInventoryItem inventoryItemBaseUI)
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
        public void DeselectAllItem()
        {
            foreach (BaseUIInventoryItem item in listOfUIItems) item.Deselect();
        }

        #endregion
    }
}