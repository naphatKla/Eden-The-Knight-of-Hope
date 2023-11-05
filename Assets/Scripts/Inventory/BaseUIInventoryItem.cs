using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Inventory
{
    public class BaseUIInventoryItem : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IEndDragHandler,
        IDropHandler, IDragHandler 
    {
        [SerializeField] private Image itemImage;
        [SerializeField] private TMP_Text quantityText;
        [SerializeField] private Image borderImage;
        public ItemSo ItemData { get; set; }
        public InventorySo ParentInventoryData { get; set; }
        public int Index { get; set; }
        public ItemSlotType itemSlotType;
        public event Action<BaseUIInventoryItem> OnItemClicked,
            OnItemDroppedOn,
            OnItemBeginDrag,
            OnItemEndDrag,
            OnRightMouseBtnClick;

        public bool isEmpty = true;

        public void Awake()
        {
            ResetData();
            Deselect();
        }

        public void OnPointerClick(PointerEventData pointerData)
        {
            if (pointerData.button == PointerEventData.InputButton.Right)
                OnRightMouseBtnClick?.Invoke(this);
            else if (pointerData.button == PointerEventData.InputButton.Left)
                OnItemClicked?.Invoke(this);
            else if (pointerData.button == PointerEventData.InputButton.Middle)
                ParentInventoryData.SortInventory();
        }

        public virtual void OnBeginDrag(PointerEventData eventData)
        {
            if (isEmpty) return;
            if (eventData.button != PointerEventData.InputButton.Left) return;
            OnItemBeginDrag?.Invoke(this);
        }

        public virtual void OnEndDrag(PointerEventData eventData)
        {
            OnItemEndDrag?.Invoke(this);
        }

        public virtual void OnDrop(PointerEventData eventData)
        {
            BaseUIInventoryItem droppedItem = eventData.pointerDrag.GetComponent<BaseUIInventoryItem>();
            if (droppedItem == null) return;
            if (droppedItem.ParentInventoryData != ParentInventoryData)
            { 
                if (droppedItem.isEmpty) return;
                droppedItem.ParentInventoryData.SwapItemsMoveBetweenInventories(ParentInventoryData, droppedItem.Index, Index);
                return;
            }
            
            OnItemDroppedOn?.Invoke(this);
        }

        public void OnDrag(PointerEventData eventData)
        {
        }

        #region Methods

        /// <summary>
        /// Reset the item data.
        /// </summary>
        public void ResetData()
        {
            itemImage.gameObject.SetActive(false);
            ItemData = null;
            isEmpty = true;
        }

        /// <summary>
        /// Deselect the item (hide the border).
        /// </summary>
        public void Deselect()
        {
            borderImage.enabled = false;
        }

        /// <summary>
        /// Set the item data.
        /// </summary>
        /// <param name="sprite">Item sprite.</param>
        /// <param name="quantity">Item amount.</param>
        public void SetData(ItemSo itemData, int quantity)
        {
            itemImage.gameObject.SetActive(true);
            //itemSlotType = itemData.ItemSlotType;
            itemImage.sprite = itemData.ItemImage;
            quantityText.text = quantity + "";
            ItemData = itemData;
            isEmpty = false;
        }
        
        public void SetData(Sprite sprite, int quantity)
        {
            itemImage.gameObject.SetActive(true);
            itemImage.sprite = sprite;
            quantityText.text = quantity + "";
            isEmpty = false;
        }

        /// <summary>
        /// Select the item (show the border).
        /// </summary>
        public void Select()
        {
            borderImage.enabled = true;
        }

        #endregion
    }
}