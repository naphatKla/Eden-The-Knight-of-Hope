using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class InventoryController : MonoBehaviour
{
    [SerializeField] private UIInventoryPage inventoryUI;
    [SerializeField] private InventorySO inventoryData;
    [SerializeField] private KeyCode Key;

   

    public void Start()
    {
        PrepareUI();
        //inventoryData.Initialize();
    }

    private void PrepareUI()
    {
        inventoryUI.InitializeInventoryUI(inventoryData.Size);
        this.inventoryUI.OnDescriptionRequested += HandleDescriptionRequest;
        this.inventoryUI.OnSwapItem += HandleSwapItem;
        this.inventoryUI.OnStarDragging += HandleDragging;
        this.inventoryUI.OnItemActionRequested += HandleItemActionRequest;
    }

    private void HandleItemActionRequest(int itemIndex)
    {
        
    }

    private void HandleDragging(int itemIndex)
    {
        
    }

    private void HandleSwapItem(int itemIndex_1, int itemIndex_2)
    {
       
    }

    private void HandleDescriptionRequest(int itemIndex)
    {
        InventoryItem inventoryItem = inventoryData.GetItemAT(itemIndex);
        if (inventoryItem.IsEmpty)
        {
            inventoryUI.ResetSeliction();
            return;
        }
        ItemSO item = inventoryItem.item;
        inventoryUI.UpdateDescription(itemIndex, item.ItemImage,
            item.name, item.Description);
    }

    public void Update()
    {
        if (Input.GetKeyDown(Key))
        {
            if (inventoryUI.isActiveAndEnabled == false)
            {
                inventoryUI.Show();
                foreach (var item in inventoryData.GetCurrentInventory())
                {
                    inventoryUI.UpdateData(item.Key, 
                        item.Value.item.ItemImage, 
                        item.Value.quantity);
                }
            }
            else
            {
                inventoryUI.Hide();
                
            }
        }
    }
}
