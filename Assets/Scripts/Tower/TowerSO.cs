using System;
using System.Collections.Generic;
using System.Linq;
using Inventory;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public struct TowerRepairState
{
    public int hpGreaterThan;
    public int repairCost;
    public InventoryItem[] repairItems;
}

[CreateAssetMenu]
public class TowerSO : ScriptableObject
{
    public string towerName;
    public string towerDescription;
    public Tower.Tower towerPrefab;
    public int cost;
    public Sprite towerImage;
    public TowerSO upgradeTower;
    public InventoryItem[] requireItems;
    public TowerRepairState[] repairStates;
    
    public bool CheckRecipe()
    {
        List<InventoryItem> playerItems = PlayerInventoryController.Instance.InventoryData.GetAllItems();
        foreach (InventoryItem requireItem in requireItems)
        {
            if (playerItems.All(item => item.item != requireItem.item)) return false;
            if (playerItems.Where(item => item.item == requireItem.item).Sum(item => item.quantity) < requireItem.quantity) return false;
        }
        return true;
    }
}
