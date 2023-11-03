using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Inventory
{
    public class UIInventoryItem : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IEndDragHandler,
        IDropHandler, IDragHandler
    {
        [SerializeField] private Image itemImage;
        [SerializeField] private TMP_Text quantityText;
        [SerializeField] private Image borderImage;
        public InventorySo ParentInventoryData { get; set; }
        public int Index { get; set; }
        public event Action<UIInventoryItem> OnItemClicked,
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
            else
                OnItemClicked?.Invoke(this);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (isEmpty) return;
            OnItemBeginDrag?.Invoke(this);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            OnItemEndDrag?.Invoke(this);
        }

        public void OnDrop(PointerEventData eventData)
        {
            UIInventoryItem droppedItem = eventData.pointerDrag.GetComponent<UIInventoryItem>();
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