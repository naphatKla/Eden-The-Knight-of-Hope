using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Inventory
{
    public class UIInventoryItem : MonoBehaviour, IPointerClickHandler, 
        IBeginDragHandler, IEndDragHandler, IDropHandler, IDragHandler
    {
        [SerializeField] private Image itemImage;
        [SerializeField] private TMP_Text quantityText;
        [SerializeField] private Image borderImage;
        public event Action<UIInventoryItem> OnItemClicked, OnItemDroppedOn, OnItemBeginDrag, OnItemEndDrag, OnRightMouseBtnClick;
        private bool _isEmpty = true;
    
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
            if (_isEmpty) return;
            OnItemBeginDrag?.Invoke(this);
        }
    
        public void OnEndDrag(PointerEventData eventData)
        {
            OnItemEndDrag?.Invoke(this);
        }
    
        public void OnDrop(PointerEventData eventData)
        {
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
            _isEmpty = true;
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
            _isEmpty = false;
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

