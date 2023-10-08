using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class InventoryController : MonoBehaviour
{
    [SerializeField] private UIInventoryPage inventoryUI;
    [SerializeField] private KeyCode Key;

    public int inventorySize = 10;

    public void Start()
    {
        inventoryUI.InitializeInventoryUI(inventorySize); 
    }

    public void Update()
    {
        if (Input.GetKeyDown(Key))
        {
            if (inventoryUI.isActiveAndEnabled == false)
            {
                inventoryUI.Show();
            }
            else
            {
                inventoryUI.Hide();
            }
        }
    }
}
