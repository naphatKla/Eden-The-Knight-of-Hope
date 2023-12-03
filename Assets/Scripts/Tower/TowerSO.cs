using System;
using System.Collections.Generic;
using System.Linq;
using Inventory;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public struct TowerRepairState
{
    public int hpPercentGreaterThan;
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
    public int tier;
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
    
    public bool CheckRepairRecipe(float hp)
    {
        List<InventoryItem> playerItems = PlayerInventoryController.Instance.InventoryData.GetAllItems();
        foreach (TowerRepairState repairState in repairStates)
        {
            if (hp < repairState.hpPercentGreaterThan) continue;
            foreach (InventoryItem repairItem in repairState.repairItems)
            {
                if (playerItems.All(item => item.item != repairItem.item)) return false;
                if (playerItems.Where(item => item.item == repairItem.item).Sum(item => item.quantity) < repairItem.quantity) return false;
            }
            return true;
        }
        return false;
    }
    
    public TowerRepairState GetRepairState(float hp)
    {
        TowerRepairState result = new TowerRepairState();
        foreach (TowerRepairState repairState in repairStates)
        {
            Debug.Log($"{hp} : {repairState.hpPercentGreaterThan}");
            if (hp < repairState.hpPercentGreaterThan) continue;
            return repairState;
        }
        return result;
    }
}
