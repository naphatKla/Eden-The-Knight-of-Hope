using System;
using Inventory;
using UnityEngine;
using UnityEngine.EventSystems;

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
        if (itemSlotType == ItemSlotType.QuickSlot) return;
        equipmentItemData = ItemData as EquipmentItemSO;
        equipmentItemData?.AddStats();
    }
}
