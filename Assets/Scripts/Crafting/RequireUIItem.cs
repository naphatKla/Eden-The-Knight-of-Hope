using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using Inventory;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RequireUIItem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private Image itemImage;
    [SerializeField] private TextMeshProUGUI quantityText;
    [SerializeField] private TextMeshProUGUI availableText;
    
    public void SetData(InventoryItem item, int quantity, int available)
    {
        nameText.text = item.item.name;
        itemImage.sprite = item.item.ItemImage;
        quantityText.text = $"x{quantity}";
        availableText.text = quantity > available ? $"<color=red>x{available}\navailable</color>" : $"<color=green>x{available}\navailable</color>";
    }

}
