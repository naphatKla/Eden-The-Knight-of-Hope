using System.Collections.Generic;
using System.Linq;
using Inventory;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentInventoryController : BaseInventoryController<UIEquipmentInventoryPage>
{
    [SerializeField] private Image weaponSlot;
    [SerializeField] private List<Image> quickSlots = new List<Image>();
    [SerializeField] private List<TextMeshProUGUI> quickSlotQuantityTexts= new List<TextMeshProUGUI>();
    private float _quickSlot1CoolDown, _quickSlot2CoolDown, _quickSlot3CoolDown;
    
    protected void Update()
    {
        QuickSlotHandle();
    }

    private void QuickSlotHandle()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            UIEquipmentInventoryItem quickSlot = inventoryUI.lisOfQuickSlots[0];
            EquipmentItemSO equipmentItemData =  quickSlot.ItemData as EquipmentItemSO;
            if (!equipmentItemData) return;
            if (_quickSlot1CoolDown > 0) return;
            _quickSlot1CoolDown = equipmentItemData.coolDown;
            equipmentItemData.AddStats();
            quickSlot.ParentInventoryData.RemoveItem(quickSlot.Index, 1);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            UIEquipmentInventoryItem quickSlot = inventoryUI.lisOfQuickSlots[1];
            EquipmentItemSO equipmentItemData =  quickSlot.ItemData as EquipmentItemSO;
            if (!equipmentItemData) return;
            if (_quickSlot2CoolDown > 0) return;
            _quickSlot2CoolDown = equipmentItemData.coolDown;
            equipmentItemData.AddStats();
            quickSlot.ParentInventoryData.RemoveItem(quickSlot.Index, 1);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            UIEquipmentInventoryItem quickSlot = inventoryUI.lisOfQuickSlots[2];
            EquipmentItemSO equipmentItemData =  quickSlot.ItemData as EquipmentItemSO;
            if (!equipmentItemData) return;
            if (_quickSlot3CoolDown > 0) return;
            _quickSlot3CoolDown = equipmentItemData.coolDown;
            equipmentItemData.AddStats();
            quickSlot.ParentInventoryData.RemoveItem(quickSlot.Index, 1);
        }
        
        _quickSlot1CoolDown = _quickSlot1CoolDown <= 0? 0 : _quickSlot1CoolDown - Time.deltaTime;
        _quickSlot2CoolDown = _quickSlot2CoolDown <= 0? 0 : _quickSlot2CoolDown - Time.deltaTime;
        _quickSlot3CoolDown = _quickSlot3CoolDown <= 0? 0 : _quickSlot3CoolDown - Time.deltaTime;
    }

    protected override void UpdateInventoryUI(Dictionary<int, InventoryItem> inventoryState)
    {
        base.UpdateInventoryUI(inventoryState);
        List<BaseUIInventoryItem> listOfUIItems = inventoryUI.listOfUIItems;
        BaseUIInventoryItem weapon = listOfUIItems.FirstOrDefault(item => item.itemSlotType == ItemSlotType.Weapon);
        weaponSlot.gameObject.SetActive(!weapon.isEmpty);
        if (!weapon.isEmpty)
            weaponSlot.sprite = weapon.ItemData.ItemImage;
        
        for (int i = 0; i < quickSlots.Count; i++)
        {
            BaseUIInventoryItem quickSlot = inventoryUI.lisOfQuickSlots[i];
            quickSlots[i].gameObject.SetActive(!quickSlot.isEmpty);
            if (quickSlot.isEmpty) continue;
            quickSlots[i].sprite = quickSlot.ItemData.ItemImage;
            quickSlotQuantityTexts[i].text = quickSlot.quantityText.text;
        }
    }

    protected override void PrepareUIEvent()
    {
        base.PrepareUIEvent();
        inventoryUI.OnDescriptionRequested += HandleDescriptionRequest;
    }

    /// <summary>
    /// send the description of the item to the inventory UI page.
    /// </summary>
    /// <param name="itemIndex">Item index in the inventory</param>
    private void HandleDescriptionRequest(int itemIndex)
    {
        InventoryItem inventoryItem = inventoryData.GetItemAt(itemIndex);
        if (inventoryItem.IsEmpty)
        {
            inventoryUI.ResetSelection();
            return;
        }

        ItemSo item = inventoryItem.item;
        inventoryUI.UpdateDescription(itemIndex, item.ItemImage, item.name, item.Description);
    }
}
