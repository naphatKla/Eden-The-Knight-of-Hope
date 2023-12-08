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
        [SerializeField] public TMP_Text quantityText;
        [SerializeField] private Image borderImage;
        [SerializeField] protected AudioClip[] clickSounds;
        [SerializeField] protected AudioClip[] dragSounds;
        [SerializeField] protected AudioClip[] dropSounds;
        public ItemSo ItemData { get; set; }
        public InventorySo ParentInventoryData { get; set; }
        public int Index { get; set; }
        public ItemSlotType itemSlotType;
        public int Quantity;
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

        public virtual void OnPointerClick(PointerEventData pointerData)
        {
            HandleClick(pointerData);
            if (pointerData.button == PointerEventData.InputButton.Middle)
                ParentInventoryData.SortInventory();
            SoundManager.Instance.RandomPlaySound(clickSounds);
        }

        public virtual void OnBeginDrag(PointerEventData eventData)
        {
            if (isEmpty) return;
            if (eventData.button != PointerEventData.InputButton.Left) return;
            OnItemBeginDrag?.Invoke(this);
            SoundManager.Instance.RandomPlaySound(dragSounds);
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
            SoundManager.Instance.RandomPlaySound(dropSounds);
        }

        public void OnDrag(PointerEventData eventData)
        {
        }

        protected void HandleClick(PointerEventData pointerData)
        {
            if (pointerData.button == PointerEventData.InputButton.Right)
                OnRightMouseBtnClick?.Invoke(this);
            else if (pointerData.button == PointerEventData.InputButton.Left)
                OnItemClicked?.Invoke(this);
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
            Quantity = 0;
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
            Quantity = quantity;
            ItemData = itemData;
            isEmpty = false;
        }
        
        public void SetData(Sprite sprite, int quantity)
        {
            itemImage.gameObject.SetActive(true);
            itemImage.sprite = sprite;
            quantityText.text = quantity + "";
            Quantity = quantity;
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