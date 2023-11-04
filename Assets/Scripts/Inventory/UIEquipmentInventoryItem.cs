using Inventory;
using UnityEngine;
using UnityEngine.EventSystems;

public enum ItemSlotType
{
    Weapon,
    Armor,
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
        equipmentItemData?.RemoveStats();
    }

    public override void OnDrop(PointerEventData eventData)
    {
        BaseUIInventoryItem droppedItem = eventData.pointerDrag.GetComponent<BaseUIInventoryItem>();
        EquipmentItemSO equipmentItemData = ItemData as EquipmentItemSO;
        if (droppedItem.itemSlotType != itemSlotType) return;
        if (!isEmpty)
        {
            equipmentItemData?.RemoveStats();
            Debug.Log("Not Empty");
        }
        base.OnDrop(eventData);
        equipmentItemData = ItemData as EquipmentItemSO;
        equipmentItemData?.AddStats();
    }
}
