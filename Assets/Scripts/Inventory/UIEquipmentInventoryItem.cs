using System;
using System.Collections.Generic;
using Inventory;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum ItemSlotType
{
    Weapon,
    Helmet,
    Armor,
    Leggings,
    Boots,
    QuickSlot,
    Other
}

public class UIEquipmentInventoryItem : BaseUIInventoryItem
{
    public Image BGSlot;
    public override void OnBeginDrag(PointerEventData eventData)
    {
        if (isEmpty) return;
        base.OnBeginDrag(eventData);
        EquipmentItemSO equipmentItemData = ItemData as EquipmentItemSO;
        if (itemSlotType == ItemSlotType.QuickSlot) return;
        equipmentItemData?.RemoveStats();
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        EquipmentItemSO equipmentItemData = ItemData as EquipmentItemSO;
        if (itemSlotType != ItemSlotType.QuickSlot)
            equipmentItemData?.AddStats();
        base.OnEndDrag(eventData);
        BGSlot.gameObject.SetActive(isEmpty);
    }

    public override void OnPointerClick(PointerEventData pointerData)
    {
        HandleClick(pointerData);
    }

    public override void OnDrop(PointerEventData eventData)
    {
        BaseUIInventoryItem droppedItem = eventData.pointerDrag.GetComponent<BaseUIInventoryItem>();
        EquipmentItemSO equipmentItemData = ItemData as EquipmentItemSO;
        if (droppedItem.ItemData.ItemSlotType != itemSlotType) return;
        if (!isEmpty && itemSlotType != ItemSlotType.QuickSlot)
        {
            equipmentItemData?.RemoveStats();
            Debug.Log("Not Empty");
        }
        base.OnDrop(eventData);
        BGSlot.gameObject.SetActive(isEmpty);
        if (itemSlotType == ItemSlotType.QuickSlot) return;
        equipmentItemData = ItemData as EquipmentItemSO;
        equipmentItemData?.AddStats();
    }
}
