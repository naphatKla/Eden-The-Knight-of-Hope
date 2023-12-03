using System;
using System.Collections.Generic;
using System.Linq;
using Inventory;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class EquipmentInventoryController : BaseInventoryController<UIEquipmentInventoryPage>
{
    [SerializeField] private Image weaponSlot;
    [SerializeField] private List<Image> quickSlots = new List<Image>();
    [SerializeField] private List<Image> quickSlotsCooldown = new List<Image>();
    [SerializeField] private List<TextMeshProUGUI> quickSlotQuantityTexts= new List<TextMeshProUGUI>();
    private float _quickSlot1CurrentCooldown, _quickSlot2CurrentCooldown, _quickSlot3CurrentCooldown;
    private float _quickSlot1Cooldown, _quickSlot2Cooldown, _quickSlot3Cooldown;
    
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
            if (_quickSlot1CurrentCooldown > 0) return;
            _quickSlot1CurrentCooldown = equipmentItemData.coolDown;
            _quickSlot1Cooldown = equipmentItemData.coolDown;
            equipmentItemData.AddStats();
            quickSlot.ParentInventoryData.RemoveItem(quickSlot.Index, 1);
            quickSlot.BGSlot.gameObject.SetActive(quickSlot.isEmpty);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            UIEquipmentInventoryItem quickSlot = inventoryUI.lisOfQuickSlots[1];
            EquipmentItemSO equipmentItemData =  quickSlot.ItemData as EquipmentItemSO;
            if (!equipmentItemData) return;
            if (_quickSlot2CurrentCooldown > 0) return;
            _quickSlot2CurrentCooldown = equipmentItemData.coolDown;
            _quickSlot2Cooldown = equipmentItemData.coolDown;
            equipmentItemData.AddStats();
            quickSlot.ParentInventoryData.RemoveItem(quickSlot.Index, 1);
            quickSlot.BGSlot.gameObject.SetActive(quickSlot.isEmpty);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            UIEquipmentInventoryItem quickSlot = inventoryUI.lisOfQuickSlots[2];
            EquipmentItemSO equipmentItemData =  quickSlot.ItemData as EquipmentItemSO;
            if (!equipmentItemData) return;
            if (_quickSlot3CurrentCooldown > 0) return;
            _quickSlot3CurrentCooldown = equipmentItemData.coolDown;
            _quickSlot3Cooldown = equipmentItemData.coolDown;
            equipmentItemData.AddStats();
            quickSlot.ParentInventoryData.RemoveItem(quickSlot.Index, 1);
            quickSlot.BGSlot.gameObject.SetActive(quickSlot.isEmpty);
        }
        
        _quickSlot1CurrentCooldown = _quickSlot1CurrentCooldown <= 0 || double.IsNaN(_quickSlot1CurrentCooldown)? 0 : _quickSlot1CurrentCooldown - Time.deltaTime;
        _quickSlot2CurrentCooldown = _quickSlot2CurrentCooldown <= 0 || double.IsNaN(_quickSlot2CurrentCooldown)? 0 : _quickSlot2CurrentCooldown - Time.deltaTime;
        _quickSlot3CurrentCooldown = _quickSlot3CurrentCooldown <= 0 || double.IsNaN(_quickSlot1CurrentCooldown)? 0 : _quickSlot3CurrentCooldown - Time.deltaTime;
        quickSlotsCooldown[0].gameObject.SetActive(_quickSlot1CurrentCooldown > 0);
        quickSlotsCooldown[1].gameObject.SetActive(_quickSlot2CurrentCooldown > 0);
        quickSlotsCooldown[2].gameObject.SetActive(_quickSlot3CurrentCooldown > 0);
        quickSlotsCooldown[0].fillAmount = _quickSlot1CurrentCooldown / _quickSlot1Cooldown;
        quickSlotsCooldown[1].fillAmount = _quickSlot2CurrentCooldown / _quickSlot2Cooldown;
        quickSlotsCooldown[2].fillAmount = _quickSlot3CurrentCooldown / _quickSlot3Cooldown;
        
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
